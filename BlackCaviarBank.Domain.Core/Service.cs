using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Core
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<SubscriptionSubscriber> SubscriptionSubscribers { get; set; } = new List<SubscriptionSubscriber>();
    }
}
