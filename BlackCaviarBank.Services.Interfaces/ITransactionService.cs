using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedList<Transaction>> GetTransactionsForCurrentUser(UserProfile user, TransactionParams transactionParams);
        Task<Transaction> GetTransaction(Guid id);
        Task<Transaction> MakeTransaction(TransactionDTO transaction, UserProfile payer);
        Task RollbackTransaction(Guid id);
    }
}
