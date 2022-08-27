using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Pushfi.Domain.Entities.Authentication
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public ApplicationUser()
        {
			this.RefreshTokens = new List<RefreshTokenEntity>();
		}

		public bool IsDeleted { get; set; } = false;

		public string AvatarColor { get; set; }

		public virtual IList<RefreshTokenEntity> RefreshTokens { get; set; }
	}
}
