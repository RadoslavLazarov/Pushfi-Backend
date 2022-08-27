using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;
using System.Text.Json.Serialization;

namespace Pushfi.Application.Common.Models.Authentication
{
    public class AuthenticateResponseModel : ModelBase
    {
        public AuthenticateResponseModel(ApplicationUser user, RoleType role, string jwtToken, string refreshToken, string fullName)
        {
            Id = user.Id;
            Email = user.Email;
            RoleType = role;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            FullName = fullName;
            AvatarColor = user.AvatarColor;
        }

        public string Email { get; set; }

        public RoleType RoleType { get; set; }

        public string RoleName => RoleType.GetName(RoleType);

        public string JwtToken { get; set; }

        //[JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public string FullName { get; set; }

        public string AvatarColor { get; set; }
    }
}
