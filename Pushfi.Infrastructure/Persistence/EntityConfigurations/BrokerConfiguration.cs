using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Infrastructure.Persistence.Extensions;

namespace Pushfi.Infrastructure.Persistence.EntityConfigurations
{
    public class BrokerConfiguration : IEntityTypeConfiguration<BrokerEntity>
    {
        public void Configure(EntityTypeBuilder<BrokerEntity> builder)
        {
            builder.HasIndex(x => x.UrlPath).IsUnique();
            builder.Property(x => x.LogoImage).HasJsonConversion();
            builder.Property(x => x.AdditionalDocument).HasJsonConversion();
        }
    }
}
