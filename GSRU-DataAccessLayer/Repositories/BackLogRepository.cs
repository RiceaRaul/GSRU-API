using Dapper;
using GSRU_API.Common.Extensions;
using GSRU_API.Common.Models;
using GSRU_Common.Models;
using GSRU_Common.Models.Boards;
using GSRU_Common.Models.Requests.Tasks;
using GSRU_Common.Models.Tasks;
using GSRU_DataAccessLayer.Common;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GSRU_DataAccessLayer.Repositories
{
    public class BackLogRepository(IDbTransaction transaction) : RepositoryBase(transaction), IBackLogRepository
    {
        private const string BOARD_GET_BACKLOG = "board_get_backlog";
        private const string UPDATE_TASKS_SPRINT_AND_INDEX = "update_tasks_sprint_and_index";

        public async Task<GenericResponse<int>> UpdateTaskSprintAndIndexAsync(TaskUpdateSprintAndIndexRequest request)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("TaskId", typeof(int));
                dataTable.Columns.Add("SprintId", typeof(int));
                dataTable.Columns.Add("Index", typeof(int));
                foreach (var taskUpdate in request.Tasks)
                {
                    dataTable.Rows.Add(taskUpdate.TaskId, taskUpdate.SprintId, taskUpdate.Index);
                }
                var parameters = new DynamicParameters();
                parameters.Add("@TaskUpdates", dataTable.AsTableValuedParameter("dbo.TaskUpdateType"));
                var result = await Connection.ExecuteAsync(
                    sql: UPDATE_TASKS_SPRINT_AND_INDEX,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<int>
                {
                    Data = result
                };
            }
            catch (SqlException ex) when (ex.Number == (int)CustomSqlException.UserNotExist || ex.Number == (int)CustomSqlException.PasswordIncorrect)
            {
                var enumValue = Enum.Parse<CustomSqlException>(ex.Number.ToString());
               return GenerateGenericError.Generate<GenericResponse<int>>(HttpStatusCode.Unauthorized, enumValue.ToDescriptionString());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                   return GenerateGenericError.Generate<GenericResponse<int>>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
               return GenerateGenericError.GenerateInternalError<GenericResponse<int>>(ex.Message);
            }
        }

        public async Task<BackLogDto> GetBacklogAsync(int boardId)
        {
            try
            {
                using var result = await Connection.QueryMultipleAsync(
                       sql: BOARD_GET_BACKLOG,
                       param: new { p_board_id = boardId },
                       commandType: CommandType.StoredProcedure,
                       commandTimeout: 20,
                       transaction: Transaction
                   );
                var sprints = result.Read<SprintDto>().ToList();
                var tasks = result.Read<TaskBackLogDto>().ToList();

                var backlog = new BackLogDto
                {
                    Sprints = sprints
                };
                foreach (var task in tasks)
                {
                    task.Children = tasks.Where(t => t.ParentId == task.Id);
                }

                foreach (var sprint in backlog.Sprints)
                {
                    sprint.Tasks = tasks.Where(t => t.SprintId == sprint.Id && t.ParentId is null).OrderBy(x=>x.Index);
                }
                backlog.Sprints = backlog.Sprints.OrderByDescending(x => x.Number);
                return backlog;
            }
            catch (SqlException ex) when (ex.Number == (int)CustomSqlException.UserNotExist || ex.Number == (int)CustomSqlException.PasswordIncorrect)
            {
                var enumValue = Enum.Parse<CustomSqlException>(ex.Number.ToString());
                return GenerateGenericError.Generate<BackLogDto>(HttpStatusCode.Unauthorized, enumValue.ToDescriptionString());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<BackLogDto>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<BackLogDto>(ex.Message);
            }
        }

    }
}
