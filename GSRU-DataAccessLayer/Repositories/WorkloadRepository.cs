using Dapper;
using GSRU_API.Common.Models;
using GSRU_Common.Models;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;
using System.Net;

namespace GSRU_DataAccessLayer.Repositories
{
    public class WorkloadRepository(IDbTransaction transaction) : RepositoryBase(transaction), IWorkloadRepository
    {
        private const string GET_WORKLOAD_BY_SPRINT_ID = "get_workload_by_sprint_id";
        private const string UPSERT_WORKLOAD = "upsert_workload";
        private const string UPSERT_WORKLOAD_DATA = "upsert_workload_data";

        public async Task<WorkLoadData> GetWorkloadAsync(int sprint_id)
        {
            try
            {
                using var result = await Connection.QueryMultipleAsync(
                       sql: GET_WORKLOAD_BY_SPRINT_ID,
                       param: new { sprint_id },
                       commandType: CommandType.StoredProcedure,
                       commandTimeout: 20,
                       transaction: Transaction
                   );
                var workload = await result.ReadFirstAsync<WorkLoadData>();
                var workloadInformation = await result.ReadAsync<WorkloadDto>();
                var workLoadGroup = workloadInformation.GroupBy(x => x.Employee);
                foreach(var info in workLoadGroup)
                {
                    var firstInformation = info.First();
                    dynamic workloadInfo = new Workload
                    {
                        Id = firstInformation.Id,
                        Employee = firstInformation.Employee,
                        EmployeeId = firstInformation.EmployeeId,
                        Hour = firstInformation.Hour,
                        Total = firstInformation.Total,
                    };
                    foreach (var infoGroup in info)
                    {
                        workloadInfo[infoGroup.Day] = infoGroup.DayHours;
                    }

                    workload.Data.Add(workloadInfo);
                }
                
                return workload;
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<WorkLoadData>(ex.Message);
            }
        }

        public async Task<GenericResponse<int>> UpsertWorkload(WorkLoadData request)
        {
            try
            {
                int id = request.Id == -1 ? 0 : request.Id;
                var parameters = new DynamicParameters(new
                {
                    id,
                    sprint_id = request.SprintId,
                    total_hours = request.TotalHours,
                    total_hours_support = request.TotalHoursSupport,
                    support_percent = request.SupportPercent,
                    date_start = request.DateStart,
                    date_end = request.DateEnd,
                });
                var result = await Connection.QueryFirstAsync<int>(
                    sql: UPSERT_WORKLOAD,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<int> { Data = result };
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<int>>(ex.Message);
            }
        }

        public async Task<GenericResponse<int>> CreateUpdateWorkloadAsync(Workload workload, int workload_id)
        {
            try
            {
                int? id = workload.Id == -1 ? null : workload.Id;
                var parameters = new DynamicParameters(new
                {
                    id,
                    workload_id,
                    employee_id = workload.EmployeeId,
                    hours_day = workload.Hour
                });
                var result = await Connection.QueryFirstAsync<int>(
                    sql: UPSERT_WORKLOAD_DATA,
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<int> { Data = result };
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<int>>(ex.Message);
            }
        }

        public async Task<GenericResponse<bool>> DeleteAllWorkloadInformationByWorkloadDataId(int workload_data_id)
        {
            try
            {

                await Connection.ExecuteAsync(
                    sql: "DELETE FROM [workload_information] where workload_data_id = @workload_data_id",
                    param: new { workload_data_id },
                    commandType: CommandType.Text,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<GenericResponse<bool>>(HttpStatusCode.NotFound, "WORKLOAD_INFORMATION_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>(ex.Message);
            }
        }

        public async Task<GenericResponse<bool>> InsertWorkloadInformation(int workload_data_id, int day, double day_hours)
        {
            try
            {
                await Connection.ExecuteAsync(
                    sql: @"INSERT INTO [workload_information] ([workload_data_id],[day],[day_hours])VALUES(@workload_data_id, @day, @day_hours)",
                    param: new { workload_data_id, day, day_hours },
                    commandType: CommandType.Text,
                    commandTimeout: 20,
                    transaction: Transaction
                );

                return new GenericResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<GenericResponse<bool>>(HttpStatusCode.NotFound, "WORKLOAD_INFORMATION_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>(ex.Message);
            }
        }


    }
}
