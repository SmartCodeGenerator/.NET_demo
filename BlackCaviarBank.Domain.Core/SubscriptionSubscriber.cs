namespace BlackCaviarBank.Domain.Core
{
    public class SubscriptionSubscriber
    {
        public string SubscriberId { get; set; }
        public UserProfile Subscriber { get; set; }

        public int SubscriptionId { get; set; }
        public Service Subscription { get; set; }
    }
}
