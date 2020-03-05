using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts(UserProfile currentUser);
        Task<Account> GetAccount(Guid id);
        Task CreateAccount(AccountDTO account, UserProfile currentUser);
        Task UpdateAccount(Guid id, AccountDTO account);
        void DeleteAccount(Guid id);
    }
}
