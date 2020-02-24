using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackCaviarBank.Infrastructure.Data.EntityConfigurations
{
    class SubscriptionSubscriberConfiguration : IEntityTypeConfiguration<SubscriptionSubscriber>
    {
        public void Configure(EntityTypeBuilder<SubscriptionSubscriber> builder)
        {
            builder.HasKey(ss => new { ss.SubscriberId, ss.SubscriptionId });

            builder.HasOne(ss => ss.Subscriber).WithMany(up => up.SubscriptionSubscribers).HasForeignKey(ss => ss.SubscriberId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ss => ss.Subscription).WithMany(serv => serv.SubscriptionSubscribers).HasForeignKey(ss => ss.SubscriptionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
