using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Models.Authentication
{
    public class MeResponseModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public RoleType RoleType { get; set; }

        public string RoleName => RoleType.GetName(RoleType);

        public string FullName { get; set; }

        public string AvatarColor { get; set; }
    }
}
