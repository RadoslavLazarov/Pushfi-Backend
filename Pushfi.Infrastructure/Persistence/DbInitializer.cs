using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;

namespace Pushfi.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async void Seed(WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

			dbContext.Database.Migrate();

            #region User Roles

            var roles = Enum.GetValues(typeof(RoleType)).Cast<RoleType>();

			foreach (var role in roles)
			{
				var roleName = role.ToString();
				var roleExsists = await roleManager.RoleExistsAsync(roleName);

				if (!roleExsists)
				{
					await roleManager.CreateAsync(new ApplicationRole(roleName));
				}
			}

			#endregion
		}
    }
}
