using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.AccountNumber).HasMaxLength(20).IsRequired();
            builder.HasIndex(a => a.AccountNumber).IsUnique();
            builder.Property(a => a.Name).HasMaxLength(30).IsRequired();
            builder.Property(a => a.OpeningDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(a => a.Balance).HasDefaultValue(0).IsRequired();
            builder.Property(a => a.InterestRate).HasDefaultValue(0.1).IsRequired();
            builder.Property(a => a.IsBlocked).HasDefaultValue(false);

            builder.HasOne(a => a.Owner).WithMany(up => up.Accounts).HasForeignKey(a => a.OwnerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
