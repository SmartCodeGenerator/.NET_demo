using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class AccountRepository : BaseRepository<Account>
    {
        public AccountRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public virtual async Task<Account> GetByNumber(string number)
        {
            var result = await dbSet.FirstAsync(a => a.AccountNumber.Equals(number));
            return result;
        }
    }
}
