using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Text).HasMaxLength(150).IsRequired();
            builder.Property(n => n.Time).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(n => n.Sender).HasMaxLength(30).IsRequired();

            builder.HasOne(n => n.Receiver).WithMany(up => up.Notifications).HasForeignKey(n => n.ReceiverId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(n => n.Sender).WithMany(serv => serv.Notifications).HasForeignKey(n => n.SenderId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
