using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class UserProfileRepository : BaseRepository<UserProfile>
    {
        public UserProfileRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
