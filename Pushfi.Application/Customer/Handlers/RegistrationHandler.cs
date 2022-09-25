using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Common.Models.Enfortra;
using Pushfi.Application.Customer.Commands;
using Pushfi.Common.Constants.User;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pushfi.Application.Customer.Handlers
{
    public class RegistrationHandler : IRequestHandler<RegistrationCommand, RegistrationResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEnfortraService _enfortraService;
        private readonly IUserService _userService;
        private readonly JwtConfiguration _jwtConfiguration;

        public RegistrationHandler(
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IMapper mapper,
            IEnfortraService enfortraService,
            IUserService userService,
            IOptionsMonitor<JwtConfiguration> optionsMonitor)
        {
            this._userManager = userManager;
            this._context = context;
            this._mapper = mapper;
            this._enfortraService = enfortraService;
            this._userService = userService;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task<RegistrationResponseModel> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                throw new BusinessException(Strings.EmailAlreadyExsists);
            }

            var enfortraModel = this._mapper.Map<CreateNewUserEnrollmentModel>(request);
            var user = this._mapper.Map<ApplicationUser>(request);

            user.SecurityStamp = Guid.NewGuid().ToString();
            user.AvatarColor = new UserEntityConstants().GenerateRandomAvatarColor();

            long EnfortraUserID = 0;
            // Check if user exsists in Enfortra
            try
            {
                EnfortraUserID = await this._enfortraService.CreateNewUserEnrollmentAsync(enfortraModel);
            }
            catch (BusinessException ex)
            {
                if (ex.Message == Strings.EnfortraUserExsists)
                {
                    throw new BusinessException(ex.Message);
                }

                throw new BusinessException();
            }

            // Create user
            var userResult = await this._userManager.CreateAsync(user, request.Password);
            if (!userResult.Succeeded)
            {
                throw new Exception(Strings.SomethingWentWrong);
            }

            // Assign user to role
            var newUser = await this._userManager.FindByEmailAsync(user.Email);
            await this._userManager.AddToRoleAsync(newUser, RoleType.Customer.ToString());

            // Create customer
            var customer = this._mapper.Map<CustomerEntity>(request);
            customer.UserId = newUser.Id;
            customer.Broker = this._context.Broker
                .Where(x => x.UrlPath == request.BrokerPath)
                .FirstOrDefault();
            customer.EnfortraUserID = EnfortraUserID;
            customer.ProcessStatus = ProcessStatus.Registration;
            customer.CreatedAt = DateTimeOffset.Now;

            this._context.Customer.Add(customer);
            this._context.SaveChanges();

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtConfiguration.Secret));

            var token = new JwtSecurityToken(
                issuer: this._jwtConfiguration.ValidIssuer,
                audience: this._jwtConfiguration.ValidAudience,
                expires: DateTime.UtcNow.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            // TODO: implement refresh token
            return new RegistrationResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = userRoles[0],
                CreditReportUrl = (await this._enfortraService.GetCreditReportDetailsAsync(user.Email)).CreditReportUrl
            };
        }
    }
}
