using MediatR;
using Microsoft.AspNetCore.Identity;
using Pushfi.Application.Authentication.Commands;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Exceptions;
using System.Security;

namespace Pushfi.Application.Authentication.Handlers
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, RevokeTokenResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IAuthenticationService _authenticationService;

        public RevokeTokenHandler(
            UserManager<ApplicationUser> userManager,
            IAuthenticationService authenticationService)
        {
            this._userManager = userManager;
            this._authenticationService = authenticationService;
        }

        public async Task<RevokeTokenResponseModel> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                throw new InvalidModelStateException("Token is required");
            }

            var user = this._authenticationService.GetUserByRefreshToken(request.Token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

            if (!refreshToken.IsActive)
            {
                throw new SecurityException("Invalid token");
            }

            // revoke token and save
            this._authenticationService.RevokeRefreshToken(refreshToken, request.IpAddress, "Revoked without replacement");
            await this._userManager.UpdateAsync(user);

            return new RevokeTokenResponseModel() { Message = "Token revoked" };
        }
    }
}
