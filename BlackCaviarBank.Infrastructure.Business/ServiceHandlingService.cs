using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class ServiceHandlingService : IServiceHandlingService
    {
        public List<Service> GetSubscriptions(string userId, IEnumerable<Service> services)
        {
            var subcriptions = new List<Service>();

            foreach (var service in services)
            {
                var relations = service.SubscriptionSubscribers.Where(ss => ss.SubscriberId == userId);

                foreach (var rel in relations)
                {
                    subcriptions.Add(rel.Subscription);
                }
            }

            return subcriptions;
        }

        public bool Subscribe(Service service, UserProfile subscriber, Card card, IOperationService operationService)
        {
            var rel = new SubscriptionSubscriber { Subscriber = subscriber, Subscription = service };

            if (operationService.PayForSubscription(card, service))
            {
                subscriber.SubscriptionSubscribers.Add(rel);
                service.SubscriptionSubscribers.Add(rel);

                return true;
            }
            return false;
        }

        public bool Unsubscribe(Service service, UserProfile user)
        {
            var rel = user.SubscriptionSubscribers.First(ss => ss.SubscriptionId == service.ServiceId && ss.SubscriberId.Equals(user.Id));

            if (rel != null)
            {
                service.SubscriptionSubscribers.Remove(rel);
                user.SubscriptionSubscribers.Remove(rel);

                return true;
            }
            return false;
        }
    }
}
