using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class TransactionRepository : BaseRepository<Transaction>
    {
        public TransactionRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
