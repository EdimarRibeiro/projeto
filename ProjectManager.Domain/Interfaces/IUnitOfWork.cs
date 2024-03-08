using System;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
        Task Rollback();
    }
}
