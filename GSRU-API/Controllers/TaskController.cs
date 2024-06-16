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

        [HttpPost]
        [Route("add-task-attachments")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddTaskAttachments([FromForm] TaskAttachmentsRequest request)
        {
            var authorId = GetUserId();
            var result = await _taskService.AddTaskAttachments(request, authorId);
            return SetResult(result);
        }

        [HttpGet]
        [Route("download-task-attachment/{id:int}")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        public async Task<IActionResult> DownlloadTaskAttachment([FromRoute] int id)
        {
            var result = await _taskService.GetTaskAttachments(id);
            if(result.ApiError is not null)
            {
                return SetResult(result);
            }

            return File(result.Data.Bytes, "application/octet-stream", result.Data.FileName);
        }

        [HttpPost]
        [Route("add-task-log-work")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(GenericResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddTaskLogWork([FromBody] TaskWorkLogRequest request)
        {
            request.EmployeeId = GetUserId();
            var result = await _taskService.AddTaskLogWork(request);
            return SetResult(result);
        }
    }
}
