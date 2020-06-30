using BlackCaviarBank.Domain.Core.QueryParams;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<PagedList<TEntity>> GetAll(QueryParams queryParams = null);
        Task<TEntity> GetById(Guid id);
        Task<PagedList<TEntity>> Get(Func<TEntity, bool> predicate, QueryParams queryParams = null);
        Task Create(TEntity item);
        void Delete(Guid id);
        void Update(TEntity entity);
        Task<TEntity> GetByCriterion(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    }
}
