using GSRU_DataAccessLayer.Repositories.Interfaces;

namespace GSRU_DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITestRepository TestRepository { get; }
        void Commit();
    }
}
