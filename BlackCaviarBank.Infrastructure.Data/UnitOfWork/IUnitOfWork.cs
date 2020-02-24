using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChanges();
    }
}
