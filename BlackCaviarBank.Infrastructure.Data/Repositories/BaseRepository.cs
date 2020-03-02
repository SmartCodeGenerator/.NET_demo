using BlackCaviarBank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        private readonly ApplicationContext applicationContext;
        private readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            dbSet = applicationContext.Set<TEntity>();
        }

        public async Task Create(TEntity item)
        {
            await dbSet.AddAsync(item);
        }

        public void Delete(Guid id)
        {
            TEntity entity = dbSet.Find(id);

            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            dbSet.Load();

            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(TEntity item)
        {
            applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
