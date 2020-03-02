using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class NotificatiobRepository : BaseRepository<Notification>
    {
        public NotificatiobRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
