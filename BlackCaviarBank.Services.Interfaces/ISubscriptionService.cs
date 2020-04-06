using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<PagedList<Service>> GetServices(ServiceParams serviceParams);
        Task<Service> GetService(Guid id);
        Task<PagedList<Service>> GetUserSubscriptions(UserProfile user, ServiceParams serviceParams);
        Task<Service> CreateService(ServiceDTO service);
        Task UpdateService(ServiceDTO service, Guid id);
        void RemoveService(Guid id);
        Task SubscribeOnService(UserProfile subscriber, Card card, Guid serviceId);
        Task UnsubscribeFromService(UserProfile subscriber, Guid id);
    }
}
