using BlackCaviarBank.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class CardRepository : BaseRepository<Card>
    {
        public CardRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public virtual async Task<Card> GetByNumber(string number)
        {
            var result = await dbSet.FirstAsync(c => c.CardNumber.Equals(number));
            return result;
        }
    }
}
