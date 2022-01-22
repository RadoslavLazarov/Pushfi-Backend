using Microsoft.EntityFrameworkCore;
using Pushfi.Domain.Entities;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Entities.Email;

namespace Pushfi.Application.Common.Interfaces
{
	public interface IApplicationDbContext
	{
		DbContext DbContext { get; }
		DbSet<CustomerEntity> Customer { get; set; }
		DbSet<EmailTemplateEntity> EmailTemplate { get; set; }
		DbSet<CustomerEmailHistoryEntity> CustomerEmailHistory { get; set; }
		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
