using GSRU_DataAccessLayer.Repositories.Interfaces;

namespace GSRU_DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        ITeamsRepository TeamsRepository { get; }
        IBackLogRepository BackLogRepository { get; }
        IWorkloadRepository WorkloadRepository { get; }
        void Commit();
    }
}
