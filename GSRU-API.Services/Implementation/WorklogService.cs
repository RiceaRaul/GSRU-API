using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_API.Services.Implementation
{
    public class WorklogService(IUnitOfWork _unitOfWork) : IWorklogService
    {
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;


        public async Task<WorkLoadData> GetWorkloadAsync(int sprint_id)
        {
            var response = await _unitOfWork.WorkloadRepository.GetWorkloadAsync(sprint_id);
            return response;
        }
    }
}
