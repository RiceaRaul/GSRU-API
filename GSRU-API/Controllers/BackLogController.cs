using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
using GSRU_Common.Models.Tasks;
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

        [HttpPost]
        [Route("create-sprint/{team_id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSprint(int team_id)
        {
            var result = await _backLogService.CreateSprint(team_id);
            return SetResult(result);
        }

        [HttpPost]
        [Route("create-task")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            request.Reporter = GetUserId();
            var result = await _backLogService.CreateTask(request);
            return SetResult(result);
        }

        [HttpGet]
        [Route("get-tasks-type")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(TaskTypeStatusResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTasksType()
        {
            var result = await _backLogService.GetTasksType();
            return SetResult(result);
        }

        [HttpGet]
        [Route("get-tasks-status/{board_id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(TaskTypeStatusResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTaskStatus(int board_id)
        {
            var result = await _backLogService.GetTaskStatus(board_id);
            return SetResult(result);
        }

        [HttpPost]
        [Route("start-sprint")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> StartSprint([FromBody] StartSprintRequest request)
        {
            var result = await _backLogService.StartSprint(request.Id, request.SprintGoal);
            return SetResult(result);
        }

        [HttpGet]
        [Route("get-active-sprint/{team_id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(SprintDtoResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveSprint(int team_id)
        {
            var result = await _backLogService.GetActiveSprint(team_id);
            return SetResult(result);
        }

        [HttpGet]
        [Route("get-board-configuration/{team_id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(BoardConfigurationDtoResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBoardConfiguration(int team_id)
        {
            var result = await _backLogService.GetBoardConfiguration(team_id);
            return SetResult(result);
        }

        [HttpPatch]
        [Route("update-task-status")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] TaskUpdateStatusRequest request)
        {
            var result = await _backLogService.UpdateTaskStatusAsync(request);
            return SetResult(result);
        }


    }
}
