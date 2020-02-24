using BlackCaviarBank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Create(TEntity item)
        {
            dbSet.Add(item);
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

        public IEnumerable<TEntity> GetAll()
        {
            dbSet.Load();

            return dbSet.AsNoTracking();
        }

        public TEntity GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public void Update(TEntity item)
        {
            applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
