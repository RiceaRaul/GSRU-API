using GSRU_API.Common.Models;
using GSRU_API.Common.Settings;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;
using GSRU_DataAccessLayer.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;

namespace GSRU_API.Services.Implementation
{
    public class TaskService(IUnitOfWork taskRepository, IOptions<AppSettings> _appSettings) : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork = taskRepository;
        private readonly AppSettings _appSettings = _appSettings.Value;

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

        public async Task<GenericResponse<bool>> AddTaskAttachments(TaskAttachmentsRequest request, int authorId)
        {

            try
            {
                var files = request.Files;
                foreach (var file in files)
                {
                    var newName = string.Concat(Guid.NewGuid().ToString(), Path.GetFileNameWithoutExtension(file.FileName), Path.GetExtension(file.FileName));
                    var fileStream = new FileStream(Path.Combine(_appSettings.UploadFilesPath, newName), FileMode.Create);
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();

                    await _unitOfWork.TaskRepository.AddTaskAttachment(request.TaskId, authorId, newName, _appSettings.UploadFilesPath);
                }

                _unitOfWork.Commit();

                return new GenericResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>("Error occurrent at add task attachments");
            }
        }

        public async Task<GenericResponse<TaskAttachmentDownloadResponse>> GetTaskAttachments(int attachId)
        {
            var result = await _unitOfWork.TaskRepository.GetTaskAttachments(attachId);
            if(result is { ApiError: not null })
            {
                return GenerateGenericError.Generate<GenericResponse<TaskAttachmentDownloadResponse>>(result.StatusCode,result.ApiError.Message,result.ApiError.Data);
            }

            var filePath = Path.Combine(_appSettings.UploadFilesPath, result.Data!.FileName);
            if (!File.Exists(filePath))
            {
                return GenerateGenericError.Generate<GenericResponse<TaskAttachmentDownloadResponse>>(HttpStatusCode.NotFound, "File not found", null);

            }
            var fileBytes = await File.ReadAllBytesAsync(filePath);


            return new GenericResponse<TaskAttachmentDownloadResponse> { Data = new TaskAttachmentDownloadResponse {
                    Bytes = fileBytes,
                    FileName = result.Data.FileName
                } 
            };
        }

        public async Task<GenericResponse<bool>> AddTaskLogWork(TaskWorkLogRequest request)
        {
            var result = await _unitOfWork.TaskRepository.AddTaskLogWork(request);
            _unitOfWork.Commit();
            return result;
        }
    }
}
