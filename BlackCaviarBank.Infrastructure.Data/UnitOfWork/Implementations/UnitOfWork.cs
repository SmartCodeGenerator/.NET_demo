using System;
using System.Threading.Tasks;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.Repositories;

namespace BlackCaviarBank.Infrastructure.Data.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext applicationContext;

        private BaseRepository<Account> accountRepository;
        private BaseRepository<Card> cardRepository;
        private BaseRepository<Notification> notificationRepository;
        private BaseRepository<Service> serviceRepository;
        private BaseRepository<Transaction> transactionRepository;
        private BaseRepository<UserProfile> userProfileRepository;

        private bool disposed = false;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public BaseRepository<Account> Accounts
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = new BaseRepository<Account>(applicationContext);
                }
                return accountRepository;
            }
        }

        public BaseRepository<Card> Cards
        {
            get
            {
                if (cardRepository == null)
                {
                    cardRepository = new BaseRepository<Card>(applicationContext);
                }
                return cardRepository;
            }
        }

        public BaseRepository<Notification> Notifications
        {
            get
            {
                if (notificationRepository == null)
                {
                    notificationRepository = new BaseRepository<Notification>(applicationContext);
                }
                return notificationRepository;
            }
        }

        public BaseRepository<Service> Services
        {
            get
            {
                if (serviceRepository == null)
                {
                    serviceRepository = new BaseRepository<Service>(applicationContext);
                }
                return serviceRepository;
            }
        }

        public BaseRepository<Transaction> Transactions
        {
            get
            {
                if (transactionRepository == null)
                {
                    transactionRepository = new BaseRepository<Transaction>(applicationContext);
                }
                return transactionRepository;
            }
        }

        public BaseRepository<UserProfile> UserProfiles
        {
            get
            {
                if (userProfileRepository == null)
                {
                    userProfileRepository = new BaseRepository<UserProfile>(applicationContext);
                }
                return userProfileRepository;
            }
        }

        public async Task SaveChanges()
        {
            await applicationContext.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    applicationContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
