using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkloadController(IWorklogService _worklogService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly IWorklogService _worklogService = _worklogService;

        [HttpGet("{sprint_id}")]
        public async Task<IActionResult> GetWorkloadAsync(int sprint_id)
        {
            var response = await _worklogService.GetWorkloadAsync(sprint_id);
            return SetResult(response);
        }

        [HttpPost("create-update-workload")]
        public async Task<IActionResult> CreateUpdateWorkloadAsync([FromBody] WorkLoadData workload)
        {
            var response = await _worklogService.CreateUpdateWorkloadAsync(workload);
            return SetResult(response);
        }
    }
}
