using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class AccountRepository : BaseRepository<Account>
    {
        public AccountRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
