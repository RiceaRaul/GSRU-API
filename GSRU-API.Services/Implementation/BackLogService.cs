using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
using GSRU_Common.Models.Tasks;
using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_API.Services.Implementation
{
    public class BackLogService(IUnitOfWork _unitOfWork) : IBackLogService
    {
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;


        public async Task<BackLogDto> GetBacklogAsync(int boardId)
        {
            var result = await _unitOfWork.BackLogRepository.GetBacklogAsync(boardId);
            return result;
        }

        public async Task<GenericResponse<int>> UpdateTaskSprintAndIndexAsync(TaskUpdateSprintAndIndexRequest request)
        {
            var result = await _unitOfWork.BackLogRepository.UpdateTaskSprintAndIndexAsync(request);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<GenericResponse<int>> CreateSprint(int team_id)
        {
            var result = await _unitOfWork.BackLogRepository.CreateSprint(team_id);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<GenericResponse<int>> CreateTask(CreateTaskRequest request)
        {
            var result = await _unitOfWork.BackLogRepository.CreateTask(request);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<TaskTypeStatusResponse> GetTasksType()
        {
            var result = await _unitOfWork.BackLogRepository.GetTasksType();
            return result;
        }

        public async Task<TaskTypeStatusResponse> GetTaskStatus(int board_id)
        {
            var result = await _unitOfWork.BackLogRepository.GetTaskStatus(board_id);
            return result;
        }

        public async Task<GenericResponse<int>> StartSprint(int sprint_id, string sprint_goal)
        {
            var result = await _unitOfWork.BackLogRepository.StartSprint(sprint_id, sprint_goal);
            _unitOfWork.Commit();
            return result;
        }
    }
}
