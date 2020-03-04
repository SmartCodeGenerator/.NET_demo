using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsForCurrentUser(UserProfile user);
        Task<Transaction> GetTransaction(Guid id);
        Task<Transaction> MakeTransaction(TransactionDTO transaction, UserProfile payer);
        Task RollbackTransaction(Guid id);
    }
}
