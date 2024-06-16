using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(ITaskService _taskService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly ITaskService _taskService = _taskService;

        [HttpPost]
        [Route("assign-employee-to-task")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignEmployeeToTask([FromBody] AssignEmployeeToTaskRequest request)
        {
            request.EmployeeId ??= GetUserId();
            var result = await _taskService.AssignEmployeeToTask(request);
            return SetResult(result);
        }

        [HttpPost]
        [Route("add-task-comments")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddTaskComments([FromBody] TaskCommentsRequest request)
        {
            request.EmployeeId = GetUserId();
            var result = await _taskService.AddTaskComments(request);
            return SetResult(result);
        }
    }
}
