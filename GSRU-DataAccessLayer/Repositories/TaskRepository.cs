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
    }
}
