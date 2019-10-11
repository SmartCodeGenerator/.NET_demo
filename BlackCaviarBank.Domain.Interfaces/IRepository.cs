using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Interfaces
{
    public interface IRepository<T, V> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(V id);
        void Create(T item);
        void Update(T item);
        void Delete(V id);
    }
}
