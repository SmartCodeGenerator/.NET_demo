using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.Repositories;
using BlackCaviarBank.Infrastructure.Data.Repositories.Extensions;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork
    {
        private readonly ApplicationContext applicationContext;

        private AccountRepository accountRepository;
        private CardRepository cardRepository;
        private NotificatiobRepository notificationRepository;
        private ServiceRepository serviceRepository;
        private TransactionRepository transactionRepository;
        private UserProfileRepository userProfileRepository;

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
                    accountRepository = new AccountRepository(applicationContext);
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
                    cardRepository = new CardRepository(applicationContext);
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
                    notificationRepository = new NotificatiobRepository(applicationContext);
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
                    serviceRepository = new ServiceRepository(applicationContext);
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
                    transactionRepository = new TransactionRepository(applicationContext);
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
                    userProfileRepository = new UserProfileRepository(applicationContext);
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
