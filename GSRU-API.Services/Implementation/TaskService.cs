using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;
using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_API.Services.Implementation
{
    public class TaskService(IUnitOfWork taskRepository) : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork = taskRepository;

        public async Task<GenericResponse<bool>> AssignEmployeeToTask(AssignEmployeeToTaskRequest request)
        {
            var result  = await _unitOfWork.TaskRepository.AssignEmployeeToTask(request.EmployeeId!.Value, request.TaskId);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<GenericResponse<bool>> AddTaskComments(TaskCommentsRequest request)
        {
            var result = await _unitOfWork.TaskRepository.AddTaskComments(request);
            _unitOfWork.Commit();
            return result;
        }
    }
}
