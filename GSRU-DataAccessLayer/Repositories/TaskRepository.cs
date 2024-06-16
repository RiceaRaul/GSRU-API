using Dapper;
using GSRU_API.Common.Models;
using GSRU_Common.Models;
using GSRU_Common.Models.Tasks;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;

namespace GSRU_DataAccessLayer.Repositories
{
    public class TaskRepository(IDbTransaction transaction) : RepositoryBase(transaction), ITaskRepository
    {
        private const string ASSIGNEMPLOYEETOTASK = "AssignEmployeeToTask";
        private const string TASK_COMMENTS = "InsertTaskComment";
        private const string TASK_ATTACHMENTS = "InsertTaskAttachment";
        private const string GET_TASK_ATTACHMENT_BY_ID = "GetAttachmentById";
        private const string INSERTWORKLOG = "InsertWorkLog";
        public async Task<GenericResponse<bool>> AssignEmployeeToTask(int employeeId, int taskId)
        {
            var parameters = new DynamicParameters(new
            {
                EmployeeId = employeeId,
                TaskId = taskId
            });

            try
            {
                await Connection.ExecuteAsync(
                    sql: ASSIGNEMPLOYEETOTASK,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool>
                {
                    Data = true
                };
            }
            catch
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>("Error occurrent at assign employee");
            }
        }

        public async Task<GenericResponse<bool>> AddTaskComments(TaskCommentsRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                request.TaskId,
                AuthorId = request.EmployeeId,
                request.Comment
            });

            try
            {
                await Connection.ExecuteAsync(
                    sql: TASK_COMMENTS,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool>
                {
                    Data = true
                };
            }
            catch
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>("Error occurrent at add task comments");
            }
        }

        public async Task<GenericResponse<bool>> AddTaskAttachment(int taskId, int authorId, string fileName, string filePath)
        {
            var parameters = new DynamicParameters(new
            {
                TaskId = taskId,
                AuthorId = authorId,
                FileName = fileName,
                FilePath = filePath
            });

            try
            {
                await Connection.ExecuteAsync(
                    sql: TASK_ATTACHMENTS,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool>
                {
                    Data = true
                };
            }
            catch
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>("Error occurrent at add task comments");
            }
        } 
        
        public async Task<GenericResponse<bool>> AddTaskLogWork(TaskWorkLogRequest request)
        {
            var parameters = new DynamicParameters(new
            {
               EmployeeId = request.EmployeeId,
               TaskId = request.TaskId,
               StartDate = request.StartDate,
               EndDate = request.EndDate,
               Description = request.Description
            });

            try
            {
                await Connection.ExecuteAsync(
                    sql: INSERTWORKLOG,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool>
                {
                    Data = true
                };
            }
            catch
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>("Error occurrent at add task comments");
            }
        }



        public async Task<GenericResponse<TaskAttachment>> GetTaskAttachments(int attachmentId)
        {
            var parameters = new DynamicParameters(new
            {
                AttachmentId = attachmentId
            });

            try
            {
                var result = await Connection.QuerySingleAsync<TaskAttachment>(
                    sql: GET_TASK_ATTACHMENT_BY_ID,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );
                return new GenericResponse<TaskAttachment>
                {
                    Data = result
                };
            }
            catch
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<TaskAttachment>>("Error occurrent at get task attachments");
            }
        }
    }
}
