using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Constants;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Broker.Commands;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities;
using Pushfi.Domain.Exceptions;

namespace Pushfi.Application.Broker.Handlers
{
    public class BrokerDataHandler : IRequestHandler<BrokerDataCommand, BrokerDataForCustomerFormModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

        public BrokerDataHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IOptionsMonitor<AzureBlobStorageConfiguration> azureBlobStorageConfiguration)
        {
            this._context = context;
            this._mapper = mapper;
            this._azureBlobStorageConfiguration = azureBlobStorageConfiguration.CurrentValue;
        }

        public async Task<BrokerDataForCustomerFormModel> Handle(BrokerDataCommand request, CancellationToken cancellationToken)
        {
            var broker = this._context.Broker.Where(x => x.UrlPath == request.BrokerPath).FirstOrDefault();
            if (broker == null)
            {
                throw new EntityNotFoundException("Broker not found!");
            }
            
            if (broker.LogoImage == null || broker.LogoImage.Url == null)
            {
                var logoString = this._azureBlobStorageConfiguration.BaseUrl + 
                    this._azureBlobStorageConfiguration.ContainerName +
                    AzureBlobStorageConstants.BrokerDefaultLogoImagePath;

                broker.LogoImage = new EntityImage()
                {
                    Url = logoString
                };
            }

            return this._mapper.Map<BrokerDataForCustomerFormModel>(broker);
        }
    }
}
