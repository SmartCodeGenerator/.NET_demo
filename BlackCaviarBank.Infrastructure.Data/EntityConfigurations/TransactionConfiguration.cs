using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.From).HasMaxLength(20).IsRequired();
            builder.Property(t => t.To).HasMaxLength(20).IsRequired();
            builder.Property(t => t.Amount).HasDefaultValue(0).IsRequired();
            builder.Property(t => t.Date).HasDefaultValue(DateTime.Now).IsRequired();

            builder.HasOne(t => t.Payer).WithMany(up => up.Transactions).HasForeignKey(t => t.PayerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
