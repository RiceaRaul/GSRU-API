using GSRU_Common.Models;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface IWorkloadRepository
    {
        Task<WorkLoadData> GetWorkloadAsync(int sprint_id);
    }
}
