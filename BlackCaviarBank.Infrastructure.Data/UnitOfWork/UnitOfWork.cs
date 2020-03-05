using BlackCaviarBank.Infrastructure.Data.Repositories.Extensions;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationContext applicationContext;

        private AccountRepository accountRepository;
        private CardRepository cardRepository;
        private NotificationRepository notificationRepository;
        private ServiceRepository serviceRepository;
        private TransactionRepository transactionRepository;
        private UserProfileRepository userProfileRepository;

        private bool disposed = false;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public AccountRepository Accounts
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

        public CardRepository Cards
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

        public NotificationRepository Notifications
        {
            get
            {
                if (notificationRepository == null)
                {
                    notificationRepository = new NotificationRepository(applicationContext);
                }
                return notificationRepository;
            }
        }

        public ServiceRepository Services
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

        public TransactionRepository Transactions
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

        public UserProfileRepository UserProfiles
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
