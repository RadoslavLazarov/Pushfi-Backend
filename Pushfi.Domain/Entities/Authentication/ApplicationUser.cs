using Microsoft.AspNetCore.Identity;
using Pushfi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities.Authentication
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        public bool IsDeleted { get; set; } = false;
	}
}
