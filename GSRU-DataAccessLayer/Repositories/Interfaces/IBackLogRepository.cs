using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface IBackLogRepository
    {
        Task<BackLogDto> GetBacklogAsync(int boardId);
        Task<GenericResponse<int>> UpdateTaskSprintAndIndexAsync(TaskUpdateSprintAndIndexRequest request);
    }
}
