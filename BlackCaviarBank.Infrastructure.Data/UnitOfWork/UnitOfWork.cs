using BlackCaviarBank.Infrastructure.Data.Repositories.Extensions;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationContext applicationContext;

        private CardRepository cardRepository;
        private UserProfileRepository userProfileRepository;

        private bool disposed = false;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
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
