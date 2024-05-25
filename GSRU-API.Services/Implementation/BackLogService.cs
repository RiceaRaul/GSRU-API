using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
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
    }
}
