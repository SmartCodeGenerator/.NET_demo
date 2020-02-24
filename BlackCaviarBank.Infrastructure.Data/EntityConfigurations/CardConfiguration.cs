using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.Property(c => c.CardNumber).HasMaxLength(16).IsRequired();
            builder.HasIndex(c => c.CardNumber).IsUnique();
            builder.Property(c => c.ExpirationDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(c => c.PaymentSystem).HasDefaultValue("Visa").IsRequired();
            builder.Property(c => c.CVV2).HasMaxLength(3).IsRequired();
            builder.Property(c => c.Balance).HasDefaultValue(0).IsRequired();
            builder.Property(c => c.IsBlocked).HasDefaultValue(false);

            builder.HasOne(c => c.Owner).WithMany(up => up.Cards).HasForeignKey(c => c.OwnerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
