using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class ServiceRepository : BaseRepository<Service>
    {
        public ServiceRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
