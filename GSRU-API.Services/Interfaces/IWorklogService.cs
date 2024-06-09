using GSRU_Common.Models;

namespace GSRU_API.Services.Interfaces
{
    public interface IWorklogService
    {
        Task<GenericResponse<bool>> CreateUpdateWorkloadAsync(WorkLoadData workload);
        Task<WorkLoadData> GetWorkloadAsync(int sprint_id);
    }
}
