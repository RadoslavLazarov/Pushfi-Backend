using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pushfi.Common.Constants.User;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Resources;

namespace Pushfi.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async void Seed(WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var appConfiguration = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<AppConfiguration>>().CurrentValue;

			dbContext.Database.Migrate();

			#region User
			var users = userManager.Users.ToList();
			var defaultAdminExsists = false;

			foreach (var user in users)
			{
				var userRoles = await userManager.GetRolesAsync(user);
				if (userRoles[0] == "Admin")
                {
					defaultAdminExsists = true;
                }

				if (string.IsNullOrEmpty(user.AvatarColor))
				{
					user.AvatarColor = new UserEntityConstants().GenerateRandomAvatarColor();
					await userManager.UpdateAsync(user);
				}
			}

			if (!defaultAdminExsists)
			{
				var user = new ApplicationUser()
				{
					UserName = appConfiguration.DefaultAdminUsername,
					Email = appConfiguration.DefaultAdminUsername,
					AvatarColor = "#e15b56"
				};

				var userResult = await userManager.CreateAsync(user, appConfiguration.DefaultAdminPassword);
				if (!userResult.Succeeded)
				{
					throw new Exception(Strings.DefaultAdminCreationFailed);
				}

				var newUser = await userManager.FindByEmailAsync(user.Email);
				await userManager.AddToRoleAsync(newUser, RoleType.Admin.ToString());
			}

			#endregion

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
