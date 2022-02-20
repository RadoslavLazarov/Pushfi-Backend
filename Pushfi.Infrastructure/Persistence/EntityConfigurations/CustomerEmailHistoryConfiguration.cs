using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pushfi.Domain.Entities.Email;
using Pushfi.Infrastructure.Persistence.Extensions;

namespace Pushfi.Infrastructure.Persistence.EntityConfigurations
{
    public class CustomerEmailHistoryConfiguration : IEntityTypeConfiguration<CustomerEmailHistoryEntity>
	{
		public void Configure(EntityTypeBuilder<CustomerEmailHistoryEntity> builder)
		{
			builder.Property(x => x.ScoreFactors).HasJsonConversion();
		}
	}
}
