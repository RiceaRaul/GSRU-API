using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<GenericResponse<bool>> AddTaskComments(TaskCommentsRequest request);
        Task<GenericResponse<bool>> AssignEmployeeToTask(int employeeId, int taskId);
    }
}
