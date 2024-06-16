using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<GenericResponse<bool>> AddTaskAttachment(int taskId, int authorId, string fileName, string filePath);
        Task<GenericResponse<bool>> AddTaskComments(TaskCommentsRequest request);
        Task<GenericResponse<bool>> AddTaskLogWork(TaskWorkLogRequest request);
        Task<GenericResponse<bool>> AssignEmployeeToTask(int employeeId, int taskId);
        Task<GenericResponse<TaskAttachment>> GetTaskAttachments(int attachmentId);
    }
}
