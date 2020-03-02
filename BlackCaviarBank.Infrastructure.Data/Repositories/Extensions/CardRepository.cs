using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Infrastructure.Data.Repositories.Extensions
{
    public class CardRepository : BaseRepository<Card>
    {
        public CardRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
