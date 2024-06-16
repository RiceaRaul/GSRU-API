using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;

namespace GSRU_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<GenericResponse<bool>> AddTaskAttachments(TaskAttachmentsRequest request, int authorId);
        Task<GenericResponse<bool>> AddTaskComments(TaskCommentsRequest request);
        Task<GenericResponse<bool>> AddTaskLogWork(TaskWorkLogRequest request);
        Task<GenericResponse<bool>> AssignEmployeeToTask(AssignEmployeeToTaskRequest request);
        Task<GenericResponse<TaskAttachmentDownloadResponse>> GetTaskAttachments(int attachId);
    }
}
