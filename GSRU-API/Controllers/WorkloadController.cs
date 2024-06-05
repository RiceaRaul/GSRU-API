using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Services.Interfaces;
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
    }
}
