using Pushfi.Domain.Entities.Authentication;
using System.Text.Json.Serialization;

namespace Pushfi.Application.Common.Models.Authentication
{
    public class AuthenticateResponseModel : ModelBase
    {
        public AuthenticateResponseModel(ApplicationUser user, string role, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Email = user.Email;
            Role = role;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }

        public string Email { get; set; }

        public string Role { get; set; }

        public string JwtToken { get; set; }

        //[JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
