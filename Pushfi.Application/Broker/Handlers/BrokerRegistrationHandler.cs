using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Pushfi.Application.Broker.Commands;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.AzureBlobStorage.Interfaces;
using Pushfi.Domain.Common.Constants;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;

namespace Pushfi.Application.Broker.Handlers
{
    public class BrokerRegistrationHandler : IRequestHandler<BrokerRegistrationCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly IEmailService _emailService;
        private readonly AppConfiguration _appConfiguration;
        private readonly SendGridConfiguration _sendGridConfiguration;

        public BrokerRegistrationHandler(
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IMapper mapper,
            IAzureBlobStorageService azureBlobStorageService,
            IEmailService emailService,
            IOptionsMonitor<AppConfiguration> appConfiguration,
            IOptionsMonitor<SendGridConfiguration> sendGridConfiguration)
        {
            this._userManager = userManager;
            this._context = context;
            this._mapper = mapper;
            this._azureBlobStorageService = azureBlobStorageService;
            this._emailService = emailService;
            this._appConfiguration = appConfiguration.CurrentValue;
            this._sendGridConfiguration = sendGridConfiguration.CurrentValue;
        }

        public async Task<Unit> Handle(BrokerRegistrationCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                throw new BusinessException(Strings.EmailAlreadyExsists);
            }

            var urlPathExsists = this._context.Broker.Where(x => x.UrlPath == request.UrlPath).FirstOrDefault();
            if (urlPathExsists != null)
            {
                throw new BusinessException(Strings.UrlPathExsists);
            }

            var user = this._mapper.Map<ApplicationUser>(request);

            user.SecurityStamp = Guid.NewGuid().ToString();

            // Create user
            var userResult = await this._userManager.CreateAsync(user, request.Password);
            if (!userResult.Succeeded)
            {
                throw new Exception(Strings.SomethingWentWrong);
            }

            // Assign user to role
            var newUser = await this._userManager.FindByEmailAsync(user.Email);
            await this._userManager.AddToRoleAsync(newUser, RoleType.Broker.ToString());

            // Create broker
            var broker = this._mapper.Map<BrokerEntity>(request);
            broker.UserId = newUser.Id;
            broker.CreatedAt = DateTimeOffset.Now;

            try
            {
                if (request.LogoImageFile != null)
                {              
                    broker.LogoImage = await this._azureBlobStorageService
                        .UploadOriginalImage(
                            request.LogoImageFile,
                            DbSchemaConstants.Broker,
                            AzureBlobStorageFileType.LogoImage.ToString()
                        );
                }

                if (request.AdditionalDocumentFile != null)
                {
                    broker.AdditionalDocument = await this._azureBlobStorageService
                    .UploadAttachmentAsync(
                        request.AdditionalDocumentFile,
                        DbSchemaConstants.Broker,
                        AzureBlobStorageFileType.AdditionalDocument.ToString()
                    );
                }
            }
            catch
            {
                // Delete user if files uploading fail
                await this._userManager.DeleteAsync(newUser);
                throw new Exception(Strings.FileUploadFailed);
            }      

            await this._context.Broker.AddAsync(broker);
            await this._context.SaveChangesAsync(cancellationToken);

            var brokerEmail = new EmailModel()
            {
                Receiver = newUser.Email,
                Sender = _sendGridConfiguration.Sender,
                Subject = String.Format(Strings.BrokerRegistrationEmailSubject, broker.CompanyName),
                Message = String.Format(
                    Strings.BrokerRegistrationEmailMessage,
                    this._appConfiguration.FrontEndBaseUrl,
                    broker.UrlPath),
            };
            await _emailService.SendAsync(brokerEmail);

            return new Unit();
        }
    }
}
