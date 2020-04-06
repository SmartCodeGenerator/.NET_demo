using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IRepository<Service> serviceRepository;
        private readonly IMapper mapper;

        public SubscriptionService(IRepository<Service> serviceRepository, IMapper mapper)
        {
            this.serviceRepository = serviceRepository;
            this.mapper = mapper;
        }

        public async Task<Service> CreateService(ServiceDTO service)
        {
            var subscription = mapper.Map<Service>(service);
            await serviceRepository.Create(subscription);
            return subscription;
        }

        public async Task<Service> GetService(Guid id)
        {
            return await serviceRepository.GetById(id);
        }

        public async Task<PagedList<Service>> GetServices(ServiceParams serviceParams)
        {
            return await serviceRepository.GetAll(serviceParams);
        }

        public async Task<PagedList<Service>> GetUserSubscriptions(UserProfile user, ServiceParams serviceParams)
        {
            return await serviceRepository.Get(s => s.SubscriptionSubscribers.Count(ss => ss.SubscriberId.Equals(user.Id)) > 0, serviceParams);
        }

        public void RemoveService(Guid id)
        {
            serviceRepository.Delete(id);
        }

        public async Task SubscribeOnService(UserProfile subscriber, Card card, Guid serviceId)
        {
            var service = await serviceRepository.GetById(serviceId);
            if (!card.IsBlocked && card.Balance >= service.Price)
            {
                var link = new SubscriptionSubscriber { Subscriber = subscriber, Subscription = service };
                subscriber.SubscriptionSubscribers.Add(link);
                service.SubscriptionSubscribers.Add(link);
                card.Balance -= service.Price;
                serviceRepository.Update(service);
            }
        }

        public async Task UnsubscribeFromService(UserProfile subscriber, Guid id)
        {
            var service = await serviceRepository.GetById(id);
            var link = service.SubscriptionSubscribers.First(ss => ss.SubscriberId.Equals(subscriber.Id));
            if (link != null)
            {
                subscriber.SubscriptionSubscribers.Remove(link);
                service.SubscriptionSubscribers.Remove(link);
                serviceRepository.Update(service);
            }
        }

        public async Task UpdateService(ServiceDTO service, Guid id)
        {
            var record = await serviceRepository.GetById(id);
            mapper.Map(service, record);
            serviceRepository.Update(record);
        }
    }
}
