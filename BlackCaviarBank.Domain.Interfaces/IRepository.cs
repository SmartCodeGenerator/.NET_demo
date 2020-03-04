using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate);
        Task Create(TEntity item);
        void Update(TEntity item);
        void Delete(Guid id);
    }
}
