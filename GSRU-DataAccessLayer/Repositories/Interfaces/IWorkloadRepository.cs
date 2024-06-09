using GSRU_Common.Models;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface IWorkloadRepository
    {
        Task<GenericResponse<int>> CreateUpdateWorkloadAsync(Workload workload, int workload_id);
        Task<GenericResponse<bool>> DeleteAllWorkloadInformationByWorkloadDataId(int workload_data_id);
        Task<WorkLoadData> GetWorkloadAsync(int sprint_id);
        Task<GenericResponse<bool>> InsertWorkloadInformation(int workload_data_id, int day, double day_hours);
        Task<GenericResponse<int>> UpsertWorkload(WorkLoadData request);
    }
}
