using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasIndex(up => up.UserName).IsUnique();
            builder.HasIndex(up => up.Email).IsUnique();
            builder.Property(up => up.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(up => up.LastName).HasMaxLength(20).IsRequired();
            builder.Property(up => up.IsBanned).HasDefaultValue(false).IsRequired(false);
            builder.Property(up => up.ProfileImage).IsRequired(false);
        }
    }
}
