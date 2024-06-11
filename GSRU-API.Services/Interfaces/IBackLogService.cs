using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
using GSRU_Common.Models.Tasks;

namespace GSRU_API.Services.Interfaces
{
    public interface IBackLogService
    {
        Task<BackLogDto> GetBacklogAsync(int boardId);
        Task<GenericResponse<int>> UpdateTaskSprintAndIndexAsync(TaskUpdateSprintAndIndexRequest request);
        Task<GenericResponse<int>> CreateSprint(int team_id);
        Task<GenericResponse<int>> CreateTask(CreateTaskRequest request);
        Task<TaskTypeStatusResponse> GetTasksType();
        Task<TaskTypeStatusResponse> GetTaskStatus(int board_id);
        Task<GenericResponse<int>> StartSprint(int sprint_id, string sprint_goal);
        Task<SprintDtoResponse> GetActiveSprint(int boardId);
        Task<BoardConfigurationDtoResponse> GetBoardConfiguration(int team_id);
        Task<GenericResponse<bool>> UpdateTaskStatusAsync(TaskUpdateStatusRequest request);
    }
}
