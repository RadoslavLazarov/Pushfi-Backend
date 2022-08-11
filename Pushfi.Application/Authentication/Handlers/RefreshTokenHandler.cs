using MediatR;
using Microsoft.AspNetCore.Identity;
using Pushfi.Application.Authentication.Commands;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Authentication;
using System.Security;

namespace Pushfi.Application.Authentication.Handlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticateResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;

        public RefreshTokenHandler(
            UserManager<ApplicationUser> userManager,
            IAuthenticationService authenticationService,
            IJwtService jwtService)
        {
            this._userManager = userManager;
            this._authenticationService = authenticationService;
            this._jwtService = jwtService;
        }

        public async Task<AuthenticateResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = this._authenticationService.GetUserByRefreshToken(request.RefreshToken);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.RefreshToken);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                this._authenticationService.RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
                await this._userManager.UpdateAsync(user);
            }

            if (!refreshToken.IsActive)
            {
                throw new SecurityException("Invalid token");
            }

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = this._authenticationService.RotateRefreshToken(refreshToken, request.IpAddress);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            this._authenticationService.RemoveOldRefreshTokens(user);

            // save changes to db
            await this._userManager.UpdateAsync(user);

            // generate new jwt
            var jwtToken = await this._jwtService.GenerateJwtToken(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            return new AuthenticateResponseModel(user, userRoles[0], jwtToken, newRefreshToken.Token);
        }
    }
}
