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
        private const string TEAMS_CREATE_SPRINT = "teams_create_sprint";
        private const string TASKS_CREATE = "tasks_create";
        private const string BACKLOG_START_SPRINT = "backlog_start_sprint";

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
                    sprint.Tasks = tasks.Where(t => t.SprintId == sprint.Id && t.ParentId is null).OrderBy(x => x.Index);
                }
                backlog.Sprints = backlog.Sprints.OrderByDescending(x => x.Number);
                return backlog;
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


        public async Task<GenericResponse<int>> CreateSprint(int team_id)
        {
            try
            {
                var result = await Connection.ExecuteAsync(
                    sql: TEAMS_CREATE_SPRINT,
                    param: new { team_id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );
               
                return new GenericResponse<int> { Data = result };
            }
            catch (SqlException ex) when (ex.Number == (int)CustomSqlException.BoardNotFound)
            {
                var enumValue = Enum.Parse<CustomSqlException>(ex.Number.ToString());
                return GenerateGenericError.Generate<GenericResponse<int>>(HttpStatusCode.Unauthorized, enumValue.ToDescriptionString());
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<int>>(ex.Message);
            }
        }

        public async Task<GenericResponse<int>> CreateTask(CreateTaskRequest request)
        {
            try
            {
                var parameters = new DynamicParameters(new
                {
                    title = request.Title,
                    description = request.Description,
                    parent_id = request.ParentId,
                    reporter = request.Reporter,
                    estimate_time = request.EstimateTime,
                    story_points = request.StoryPoints,
                    task_type = request.TaskType,
                    task_status = request.TaskStatus,
                    priority = request.Priority,
                    team_id = request.TeamId,
                    sprint_id = request.SprintId
                });

                var result = await Connection.ExecuteAsync(
                   sql: TASKS_CREATE,
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
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                   return GenerateGenericError.Generate<GenericResponse<int>>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
               return GenerateGenericError.GenerateInternalError<GenericResponse<int>>(ex.Message);
            }
        }

        public async Task<TaskTypeStatusResponse> GetTasksType()
        {
            try
            {

                var result = await Connection.QueryAsync<TaskTypeStatusDto>(
                   sql: "SELECT id, name from task_types",
                   commandType: CommandType.Text,
                   commandTimeout: 20,
                   transaction: Transaction
               );
                return new TaskTypeStatusResponse
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<TaskTypeStatusResponse>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<TaskTypeStatusResponse>(ex.Message);
            }
        }

        public async Task<TaskTypeStatusResponse> GetTaskStatus(int board_id)
        {
            try
            {
                var result = await Connection.QueryAsync<TaskTypeStatusDto>(
                   sql: @"select blc.id, bl.name from [board_list_configuration] as blc 
	INNER JOIN board_lists AS bl ON bl.id = blc.board_list_id 
	INNER JOIN boards as b ON b.id = blc.board_id
	where b.team_id = @board_id order by bl.id asc",
                   param: new { board_id },
                   commandType: CommandType.Text,
                   commandTimeout: 20,
                   transaction: Transaction
               );
                return new TaskTypeStatusResponse
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<TaskTypeStatusResponse>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<TaskTypeStatusResponse>(ex.Message);
            }
        }

        public async Task<GenericResponse<int>> StartSprint(int sprint_id, string sprint_goal)
        {
            try
            {
                var result = await Connection.ExecuteAsync(
                    sql: BACKLOG_START_SPRINT,
                    param: new { sprint_id, sprint_goal},
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<int> { Data = result };
            }
            catch (SqlException ex) when (ex.Number == (int)CustomSqlException.SprintNotFound || ex.Number == (int)CustomSqlException.SprintAlreadyStarted)
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
    }
}
