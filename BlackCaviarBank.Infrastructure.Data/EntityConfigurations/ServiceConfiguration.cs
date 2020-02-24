using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(serv => serv.Name).HasMaxLength(30).IsRequired();
            builder.HasIndex(serv => serv.Name).IsUnique();
            builder.Property(serv => serv.Price).HasDefaultValue(0).IsRequired();
        }
    }
}
