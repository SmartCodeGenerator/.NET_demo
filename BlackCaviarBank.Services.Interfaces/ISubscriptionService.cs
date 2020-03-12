using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Service>> GetServices();
        Task<Service> GetService(Guid id);
        Task<IEnumerable<Service>> GetUserSubscriptions(UserProfile user);
        Task<Service> CreateService(ServiceDTO service);
        Task UpdateService(ServiceDTO service, Guid id);
        void RemoveService(Guid id);
        Task SubscribeOnService(UserProfile subscriber, Card card, Guid serviceId);
        Task UnsubscribeFromService(UserProfile subscriber, Guid id);
    }
}
