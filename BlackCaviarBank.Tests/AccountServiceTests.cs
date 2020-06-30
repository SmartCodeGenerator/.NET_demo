using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Infrastructure.Business.Resources.Mappings;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class AccountServiceTests
    {
        private readonly UserProfile activeUser;
        private readonly IMapper mapper;

        public AccountServiceTests()
        {
            activeUser = new UserProfile { Id = "17D940DE - 51AD - 4568 - 83E9 - B74E1EAA7121" };
            var accounts = new List<Account>
            {
                new Account{ AccountId = new Guid("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"), 
                    AccountNumber="11111111111111111111", 
                    Balance=100000, 
                    InterestRate=0.1, 
                    IsBlocked=false, 
                    Name="Acc1", 
                    OpeningDate=DateTime.UtcNow, 
                    Owner=activeUser },
                new Account{ AccountId = new Guid("1AEDAAF3-B3CF-4D9E-BABD-94454F2280C2"), 
                    AccountNumber="11111111111111111112", 
                    Balance=200000, 
                    InterestRate=0.15, 
                    IsBlocked=false, 
                    Name="Acc2", 
                    OpeningDate=DateTime.UtcNow, 
                    Owner=activeUser }
            };
            activeUser.Accounts = accounts;

            var myProfile = new MappingProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(config);
        }

        [Fact]
        public async void GetAccountsTest_NotNullPagedListOn1stPage()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            var accParams = new BankAccountParams { PageNumber = 1, PageSize = 1 };
            accRepoMock.Setup(repo => repo.Get(It.IsAny<Func<Account, bool>>(), accParams)).ReturnsAsync(new PagedList<Account>(activeUser.Accounts.ToList(), activeUser.Accounts.Count, accParams.PageNumber, accParams.PageSize));
            var service = new AccountService(accRepoMock.Object, null, null);

            var result = await service.GetAccounts(activeUser, accParams);

            Assert.NotNull(result);
            Assert.IsType<PagedList<Account>>(result);
            Assert.NotEmpty(result);
            Assert.Equal(activeUser.Accounts.Count, result.TotalCount);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal(activeUser.Accounts.ElementAt(1).AccountNumber, result.ElementAt(1).AccountNumber);
        }

        [Fact]
        public async void GetAccountsTest_NotNullEmptyPagedList()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            var accParams = new BankAccountParams { PageNumber = 3, PageSize = 4 };
            accRepoMock.Setup(repo => repo.Get(It.IsAny<Func<Account, bool>>(), accParams)).ReturnsAsync(new PagedList<Account>(new List<Account>(), 0, accParams.PageNumber, accParams.PageSize));
            var service = new AccountService(accRepoMock.Object, null, null);

            var result = await service.GetAccounts(activeUser, accParams);

            Assert.NotNull(result);
            Assert.IsType<PagedList<Account>>(result);
            Assert.Empty(result);
            Assert.NotEqual(activeUser.Accounts.Count, result.TotalCount);
            Assert.False(result.HasNext);
            Assert.True(result.HasPrevious);
            Assert.Equal(0, result.TotalPages);
        }

        [Fact]
        public async void GetAccount_NotNullEqualAccNumbers()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(new Account
            {
                AccountId = new Guid("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"),
                AccountNumber = "11111111111111111111",
                Balance = 100000,
                InterestRate = 0.1,
                IsBlocked = false,
                Name = "Acc1",
                OpeningDate = DateTime.UtcNow,
                Owner = activeUser
            });
            var service = new AccountService(accRepoMock.Object, null, null);

            var result = await service.GetAccount(new Guid("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"));

            Assert.NotNull(result);
            Assert.IsType<Account>(result);
            Assert.Equal(activeUser.Accounts.ElementAt(0).AccountNumber, result.AccountNumber);
        }

        [Fact]
        public async void GetAccount_NotFound()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            var service = new AccountService(accRepoMock.Object, null, null);

            var result = await service.GetAccount(new Guid("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"));

            Assert.Null(result);
        }

        [Fact]
        public async void CreateAccount_Acc3AddResultEqualsUserDataAndGeneratorProvidedNumber()
        {
            var accToAdd = new AccountDTO
            {
                Name = "Acc3",
                Balance = 40000,
                InterestRate = 0.2
            };

            var generatorMock = new Mock<IGeneratorService>();
            generatorMock.Setup(gnr => gnr.GetGeneratedAccountNumber(It.IsAny<IEnumerable<Account>>())).Returns("11111111111111111113");
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Create(It.IsAny<Account>())).Callback(() => activeUser.Accounts.Add(mapper.Map<Account>(accToAdd)));
            var service = new AccountService(accRepoMock.Object, mapper, generatorMock.Object);

            var result = await service.CreateAccount(accToAdd, activeUser);
            var last = activeUser.Accounts.Last();

            Assert.NotNull(result);
            Assert.IsType<Account>(result);
            Assert.Equal("11111111111111111113", result.AccountNumber);
            Assert.Equal(last.Name, result.Name);
            Assert.Equal(last.Balance, result.Balance);
            Assert.Equal(last.InterestRate, result.InterestRate);
            Assert.Same(activeUser, result.Owner);
        }

        [Fact]
        public async void CreateAccount_ThrowsNullReferenceExceptionWhenNullIsPassed()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Create(null)).Throws(new NullReferenceException());
            var service = new AccountService(accRepoMock.Object, mapper, new NumberGeneratorService());

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), async () => await service.CreateAccount(null, activeUser));
        }

        [Fact]
        public async void UpdateAccount_MapDataToFirstElementOfActiveUserAccounts()
        {
            var data = new AccountDTO { Name = "AccUpdated1", Balance = 20000, InterestRate = 0.25 };
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Update(It.IsAny<Account>())).Callback(() => mapper.Map(data, activeUser.Accounts.First()));
            var service = new AccountService(accRepoMock.Object, mapper, null);

            await service.UpdateAccount(Guid.Parse("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"), data);
            var result = activeUser.Accounts.First();

            Assert.Equal(Guid.Parse("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"), result.AccountId);
            Assert.Equal(data.Balance, result.Balance);
            Assert.Equal(data.InterestRate, result.InterestRate);
            Assert.Equal(data.Name, result.Name);
        }

        [Fact]
        public async void UpdateAccount_ThrowsNullReferenceExceptionWhenNullIsPassed()
        {
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Update(null)).Throws(new NullReferenceException());
            var service = new AccountService(accRepoMock.Object, mapper, null);

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), async () => await service.UpdateAccount(Guid.Parse("EA1FE648-648C-4EF9-A4E5-8CD55A238D08"), null));
        }

        [Fact]
        public void DeleteAccount_RemovesLastElementFromActiveUserAccounts()
        {
            var prevCount = activeUser.Accounts.Count();
            var accToRemove = activeUser.Accounts.Last();
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Delete(It.IsAny<Guid>())).Callback(() => activeUser.Accounts.Remove(accToRemove));
            var service = new AccountService(accRepoMock.Object, null, null);

            service.DeleteAccount(accToRemove.AccountId);

            Assert.Equal(prevCount - 1, activeUser.Accounts.Count);
            Assert.DoesNotContain(accToRemove, activeUser.Accounts);
        }

        [Fact]
        public void DeleteAccount_NoElementsDeletedWhenNullIsPassed()
        {
            var prevCount = activeUser.Accounts.Count;
            var accToRemove = activeUser.Accounts.FirstOrDefault(a => a.AccountId.Equals("766DFFEF-3412-4638-869B-A4DE3AD96F9F"));
            var accRepoMock = new Mock<IRepository<Account>>();
            accRepoMock.Setup(repo => repo.Delete(It.IsAny<Guid>())).Callback(() => activeUser.Accounts.Remove(accToRemove));
            var service = new AccountService(accRepoMock.Object, null, null);

            service.DeleteAccount(Guid.Parse("766DFFEF-3412-4638-869B-A4DE3AD96F9F"));

            Assert.Equal(prevCount, activeUser.Accounts.Count);
        }
    }
}
