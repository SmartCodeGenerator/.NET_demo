using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class TransactionRepository : IRepository<Transaction, int>
    {
        private readonly ApplicationContext context;

        public TransactionRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> GetAll() => context.Transactions;

        public Transaction Get(int id) => context.Transactions.Find(id);

        public void Create(Transaction transaction) => context.Transactions.Add(transaction);

        public void Update(Transaction transaction) => context.Transactions.Update(transaction);

        public void Delete(int id)
        {
            var transactionToRemove = Get(id);
            if (transactionToRemove != null)
            {
                context.Transactions.Remove(transactionToRemove);
            }
        }
    }
}
