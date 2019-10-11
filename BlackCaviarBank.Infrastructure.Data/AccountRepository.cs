using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

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

        public Account Get(int id) => context.Accounts.Find(id);

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
