using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Common.Interfaces;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Entities.Email;
using Pushfi.Infrastructure.Persistence.EntityConfigurations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace Pushfi.Infrastructure.Persistence
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
    {
		private readonly IHttpContextAccessor _accessor;

		public ApplicationDbContext(
			DbContextOptions<ApplicationDbContext> options,
			IHttpContextAccessor accessor) : base(options)
        {
            this.DbContext = this;
			this._accessor = accessor;
		}

        public DbContext DbContext { get; }

		public DbSet<RefreshTokenEntity> RefreshToken { get; set; }
		public DbSet<CustomerEntity> Customer { get; set; }
		public DbSet<BrokerEntity> Broker { get; set; }
		public DbSet<EmailTemplateEntity> EmailTemplate { get; set; }
		public DbSet<CustomerEmailHistoryEntity> CustomerEmailHistory { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfiguration(new CustomerConfiguration());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
		{
			var entries = ChangeTracker.Entries();
			var userClaims = this._accessor?.HttpContext?.User;

			if (userClaims != null)
            {
				var currentUserId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

				if (currentUserId != null)
				{
					foreach (var entry in entries)
					{
						if (entry.State != EntityState.Unchanged)
						{
							if (entry.Entity is ITrackable trackable)
							{
								var userId = trackable.CreatedById == Guid.Empty ? Guid.Parse(currentUserId) : trackable.CreatedById;

								switch (entry.State)
								{
									case EntityState.Added:
										trackable.CreatedAt = trackable.CreatedAt == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : trackable.CreatedAt;
										trackable.CreatedById = userId;
										break;
									case EntityState.Modified:
										trackable.ModifiedAt = DateTimeOffset.UtcNow;
										trackable.ModifiedById = userId;
										break;
									case EntityState.Deleted:
										trackable.ModifiedAt = DateTimeOffset.UtcNow;
										trackable.ModifiedById = userId;
										break;
								}

								if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
								{
									var validationContext = new ValidationContext(entry.Entity);
									Validator.ValidateObject(entry.Entity, validationContext, true);
								}
							}
						}
					}
				}
			}	
		}
	}
}
