using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class ServiceRepository : IRepository<Service, int>
    {
        private readonly ApplicationContext context;

        public ServiceRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Service> GetAll() => context.Services;
        public IEnumerable<Service> GetAllForUser(UserProfile user) => context.Services.Include(s => s.SubscriptionSubscribers).ThenInclude(sc => sc.Subscriber).Where(s => s.SubscriptionSubscribers.Any(sc => sc.SubscriberId.Equals(user.Id)));

        public Service Get(int id) => context.Services.Include(s => s.SubscriptionSubscribers).ThenInclude(sc => sc.Subscriber).FirstOrDefault(s => s.ServiceId.Equals(id));

        public void Create(Service service) => context.Services.Add(service);

        public void Update(Service service) => context.Services.Update(service);

        public void Delete(int id)
        {
            var serviceToRemove = Get(id);
            if (serviceToRemove != null)
            {
                context.Services.Remove(serviceToRemove);
            }
        }
    }
}
