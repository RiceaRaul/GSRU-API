using GSRU_DataAccessLayer.Repositories.Interfaces;

namespace GSRU_DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        void Commit();
    }
}
