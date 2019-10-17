using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class CardRepository : IRepository<Card, int>
    {
        private readonly ApplicationContext context;

        public CardRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Card> GetAll() => context.Cards;

        public IEnumerable<Card> GetAllForUser(UserProfile user)
        {
            return context.Cards.Where(a => a.OwnerId.Equals(user.Id));
        }

        public Card Get(int id) => context.Cards.Find(id);

        public Card GetForUser(UserProfile user, int id)
        {
            return context.Cards.Where(a => a.OwnerId.Equals(user.Id)).FirstOrDefault(a => a.CardId.Equals(id));
        }

        public void Create(Card card) => context.Cards.Add(card);

        public void Update(Card card) => context.Cards.Update(card);

        public void Delete(int id)
        {
            var cardToRemove = Get(id);
            if (cardToRemove != null)
            {
                context.Cards.Remove(cardToRemove);
            }
        }
    }
}
