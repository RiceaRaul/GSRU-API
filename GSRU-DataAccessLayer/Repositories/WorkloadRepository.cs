using Dapper;
using GSRU_API.Common.Models;
using GSRU_Common.Models;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;
using System.Net;
using System.Text.Json;

namespace GSRU_DataAccessLayer.Repositories
{
    public class WorkloadRepository(IDbTransaction transaction) : RepositoryBase(transaction), IWorkloadRepository
    {
        private const string GET_WORKLOAD_BY_SPRINT_ID = "get_workload_by_sprint_id";

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
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<WorkLoadData>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<WorkLoadData>(ex.Message);
            }
        }
    }
}
