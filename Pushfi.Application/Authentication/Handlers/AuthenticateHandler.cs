using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pushfi.Application.Authentication.Commands;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Application.Helpers;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pushfi.Application.Authentication.Handlers
{
    public class AuthenticateHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponseModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IEnfortraService _enfortraService;
        private readonly IUserService _userService;

        public AuthenticateHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IAuthenticationService authenticationService,
            IJwtService jwtService,
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IEnfortraService enfortraService,
            IUserService userService)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._authenticationService = authenticationService;
            this._jwtService = jwtService;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
            this._enfortraService = enfortraService;
            this._userService = userService;
        }

        public async Task<AuthenticateResponseModel> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(Strings.UserDoesNotExsists));
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new BusinessException(Strings.WrongPassword);
            }

            var role = await _userService.GetUserRoleTypeAsync(user);

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = await this._jwtService.GenerateJwtToken(user);
            var refreshToken = this._jwtService.GenerateRefreshToken(request.IpAddress);
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            this._authenticationService.RemoveOldRefreshTokens(user);

            // save changes to db
            await this._userManager.UpdateAsync(user);
            var fullName = await _userService.GetUserFullNameAsync(user);

            return new AuthenticateResponseModel(user, role, jwtToken, refreshToken.Token, fullName);
        }
    }
}
