using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IServiceHandlingService
    {
        public List<Service> GetSubscriptions(string userId, IEnumerable<Service> services);
        public bool Subscribe(Service service, UserProfile subscriber, Card card, IOperationService operationService);
        public bool Unsubscribe(Service service, UserProfile user);
    }
}
