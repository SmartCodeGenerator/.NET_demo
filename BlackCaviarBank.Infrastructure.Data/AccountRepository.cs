using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class AccountRepository : IRepository<Account, int>
    {
        private readonly ApplicationContext context;

        public AccountRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Account> GetAll() => context.Accounts;

        public IEnumerable<Account> GetAllForUser(UserProfile user)
        {
            return context.Accounts.Where(a => a.OwnerId.Equals(user.Id));
        }

        public Account Get(int id) => context.Accounts.Find(id);

        public Account GetForUser(UserProfile user, int id)
        {
            return context.Accounts.Where(a => a.OwnerId.Equals(user.Id)).FirstOrDefault(a => a.AccountId.Equals(id));
        }
        public Account GetByNumberForUser(UserProfile user, string number) => context.Accounts.Where(a => a.OwnerId.Equals(user.Id)).FirstOrDefault(a => a.AccountNumber.Equals(number));

        public void Create(Account account) => context.Accounts.Add(account);

        public void Update(Account account) => context.Accounts.Update(account);

        public void Delete(int id)
        {
            var accountToRemove = Get(id);
            if (accountToRemove != null)
            {
                context.Remove(accountToRemove);
            }
        }
    }
}
