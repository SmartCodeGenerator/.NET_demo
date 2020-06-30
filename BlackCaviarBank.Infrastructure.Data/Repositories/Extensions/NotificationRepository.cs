using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public NotificationRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
