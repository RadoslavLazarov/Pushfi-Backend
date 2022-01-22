using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Infrastructure.Persistence.Extensions;

namespace Pushfi.Infrastructure.Persistence.EntityConfigurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
	{
		public void Configure(EntityTypeBuilder<CustomerEntity> builder)
		{
			builder.Property(x => x.LoanProducts).HasJsonConversion();
			builder.HasOne(p => p.User).WithOne().OnDelete(DeleteBehavior.Cascade);
		}
	}
}
