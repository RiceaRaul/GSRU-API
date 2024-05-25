using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackLogController(IBackLogService _backLogService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly IBackLogService _backLogService = _backLogService;


        [HttpGet]
        [Route("get-backlog-team/{id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(BackLogDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBacklog(int id)
        {
            var result = await _backLogService.GetBacklogAsync(id);
            return SetResult(result);
        }

        [HttpPost]
        [Route("update-task-sprint-and-index")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateTaskSprintAndIndex([FromBody] TaskUpdateSprintAndIndexRequest request)
        {
            var result = await _backLogService.UpdateTaskSprintAndIndexAsync(request);
            return SetResult(result);
        }
    }
}
