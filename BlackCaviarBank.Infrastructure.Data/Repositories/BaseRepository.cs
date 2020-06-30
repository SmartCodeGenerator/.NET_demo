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
        protected readonly ApplicationContext applicationContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            dbSet = applicationContext.Set<TEntity>();
        }

        public virtual async Task Create(TEntity item)
        {
            await dbSet.AddAsync(item);
        }

        public virtual void Delete(Guid id)
        {
            TEntity entity = dbSet.Find(id);

            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public virtual async Task<PagedList<TEntity>> Get(Func<TEntity, bool> predicate, QueryParams queryParams = null)
        {
            dbSet.Load();
            var simpleData = await dbSet.AsNoTracking().ToListAsync();
            return queryParams != null ? await PagedList<TEntity>.ToPagedList(dbSet.AsNoTracking().Where(predicate).AsQueryable(), queryParams.PageNumber, queryParams.PageSize) :
                new PagedList<TEntity>(simpleData.Where(predicate).ToList());
        }

        public virtual async Task<PagedList<TEntity>> GetAll(QueryParams queryParams = null)
        {
            dbSet.Load();
            var simpleData = await dbSet.AsNoTracking().ToListAsync();
            return queryParams != null ? await PagedList<TEntity>.ToPagedList(dbSet.AsNoTracking().AsQueryable(), queryParams.PageNumber, queryParams.PageSize) :
                 new PagedList<TEntity>(simpleData);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public virtual async Task<TEntity> GetByCriterion(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await dbSet.FirstOrDefaultAsync(predicate);
            return entity;
        }
    }
}
