using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public NotificatiobRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
