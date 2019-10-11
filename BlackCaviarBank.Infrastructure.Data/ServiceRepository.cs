using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

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

        public Service Get(int id) => context.Services.Find(id);

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
