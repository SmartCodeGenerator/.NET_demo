using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> repository;
        private readonly IMapper mapper;
        private readonly IGeneratorService generatorService;

        public AccountService(IRepository<Account> repository, IMapper mapper, IGeneratorService generatorService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.generatorService = generatorService;
        }

        public async Task CreateAccount(AccountDTO account, UserProfile currentUser)
        {
            var record = mapper.Map<Account>(account);
            record.AccountNumber = generatorService.GetGeneratedAccountNumber(await repository.GetAll());
            record.OpeningDate = DateTime.UtcNow;
            record.Owner = currentUser;

            await repository.Create(record);
        }

        public void DeleteAccount(Guid id)
        {
            repository.Delete(id);
        }

        public async Task<Account> GetAccount(Guid id)
        {
            return await repository.GetById(id);
        }

        public IEnumerable<Account> GetAccounts(UserProfile currentUser)
        {
            return repository.Get(a => a.OwnerId.Equals(currentUser.Id));
        }

        public async Task UpdateAccount(Guid id, AccountDTO account)
        {
            var record = await repository.GetById(id);
            record = mapper.Map(account, record);

            repository.Update(record);
        }
    }
}
