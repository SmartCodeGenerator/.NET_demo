using System;
using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Core
{
    public class Service
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<SubscriptionSubscriber> SubscriptionSubscribers { get; set; } = new List<SubscriptionSubscriber>();
    }
}
