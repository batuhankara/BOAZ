using System;
using System.Threading.Tasks;

namespace Baoz.Infrastructure.SqlServer.Contracts
{
    public interface IUnitOfWork<ITContex> : IDisposable where ITContex : IDbContext
    {
        int Commit();
        Task<int> CommitAsync();
        void ChangeAutoDetectChangesStatus(bool parameter = true);
        void DetachAll();
    }
}
