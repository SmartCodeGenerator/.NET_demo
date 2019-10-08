using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
    }
}
