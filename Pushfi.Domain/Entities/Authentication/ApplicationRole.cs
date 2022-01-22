using Microsoft.AspNetCore.Identity;

namespace Pushfi.Domain.Entities.Authentication
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
