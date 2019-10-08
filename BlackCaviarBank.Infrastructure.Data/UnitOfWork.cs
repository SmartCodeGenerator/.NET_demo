using System;
using System.Threading.Tasks;
using BlackCaviarBank.Domain.Interfaces;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private AccountRepository accountRepository;
        private CardRepository cardRepository;
        private NotificationRepository notificationRepository;
        private ServiceRepository serviceRepository;
        private TransactionRepository transactionRepository;
        private UserProfileRepository userProfileRepository;
        private bool disposed = false;

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
        }

        public AccountRepository Accounts
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = new AccountRepository(context);
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
                    cardRepository = new CardRepository(context);
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
                    notificationRepository = new NotificationRepository(context);
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
                    serviceRepository = new ServiceRepository(context);
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
                    transactionRepository = new TransactionRepository(context);
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
                    userProfileRepository = new UserProfileRepository(context);
                }
                return userProfileRepository;
            }
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
