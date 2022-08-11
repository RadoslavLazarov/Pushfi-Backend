using Pushfi.Domain.Entities.Authentication;

namespace Pushfi.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(ApplicationUser user);

        string ValidateJwtToken(string token);

        RefreshTokenEntity GenerateRefreshToken(string ipAddress);
    }
}
