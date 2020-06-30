using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAccountService
    {
        Task<PagedList<Account>> GetAccounts(UserProfile currentUser, BankAccountParams bankAccountParams);
        Task<Account> GetAccount(Guid id);
        Task<Account> CreateAccount(AccountDTO account, UserProfile currentUser);
        Task UpdateAccount(Guid id, AccountDTO account);
        void DeleteAccount(Guid id);
    }
}
