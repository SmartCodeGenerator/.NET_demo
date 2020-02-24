using System;

namespace BlackCaviarBank.Domain.Core
{
    public class SubscriptionSubscriber
    {
        public string SubscriberId { get; set; }
        public UserProfile Subscriber { get; set; }

        public Guid SubscriptionId { get; set; }
        public Service Subscription { get; set; }
    }
}
