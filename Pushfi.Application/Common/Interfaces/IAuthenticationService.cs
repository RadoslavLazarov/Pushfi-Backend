using Pushfi.Domain.Entities.Authentication;

namespace Pushfi.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        void RemoveOldRefreshTokens(ApplicationUser user);

        ApplicationUser GetUserByRefreshToken(string token);

        void RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, ApplicationUser user, string ipAddress, string reason);

        RefreshTokenEntity RotateRefreshToken(RefreshTokenEntity refreshToken, string ipAddress);

        void RevokeRefreshToken(RefreshTokenEntity token, string ipAddress, string reason = null, string replacedByToken = null);
    }
}
