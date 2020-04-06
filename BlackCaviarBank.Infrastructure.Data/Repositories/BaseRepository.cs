using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<PagedList<TEntity>> Get(Func<TEntity, bool> predicate, QueryParams queryParams = null)
        {
            return queryParams != null ? await PagedList<TEntity>.ToPagedList(dbSet.AsNoTracking().Where(predicate).AsQueryable(), queryParams.PageNumber, queryParams.PageSize) :
                await PagedList<TEntity>.AsSimpleData(dbSet.AsNoTracking().Where(predicate).AsQueryable());
        }

        public async Task<PagedList<TEntity>> GetAll(QueryParams queryParams = null)
        {
            dbSet.Load();

            return queryParams != null ? await PagedList<TEntity>.ToPagedList(dbSet.AsNoTracking().AsQueryable(), queryParams.PageNumber, queryParams.PageSize) :
                await PagedList<TEntity>.AsSimpleData(dbSet.AsNoTracking().AsQueryable());
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
