using System;
using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(Guid id);
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(Guid id);
    }
}
