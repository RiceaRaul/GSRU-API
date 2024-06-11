using GSRU_API.Common.Models;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models;
using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_API.Services.Implementation
{
    public class WorklogService(IUnitOfWork _unitOfWork) : IWorklogService
    {
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;


        public async Task<WorkLoadData> GetWorkloadAsync(int sprint_id)
        {
            var response = await _unitOfWork.WorkloadRepository.GetWorkloadAsync(sprint_id);
            if (response is { ApiError: not null, ApiError.Message: "Sequence contains no elements" })
            {
                var empty_workload = new WorkLoadData
                {
                    SprintId = sprint_id,
                    Data = [],
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now
                };
                var employees = await _unitOfWork.EmployeeRepository.GetEmployeeBySprintId(sprint_id);
                foreach(var employee in employees)
                {
                    var workload = new Workload
                    {
                        Employee = $"{employee.FirstName} {employee.LastName}",
                        EmployeeId = (int)employee.Id,
                        Hour = employee.WorkHours,
                        Total = 0
                    };
                    empty_workload.Data.Add(workload);
                }

                return empty_workload;
            }
            return response;
        }

        public async Task<GenericResponse<bool>> CreateUpdateWorkloadAsync(WorkLoadData workload)
        {
            try
            {
                var upsert_workload_id = await _unitOfWork.WorkloadRepository.UpsertWorkload(workload);
                if(upsert_workload_id.ApiError != null)
                {
                    return GenerateGenericError.Generate<GenericResponse<bool>, string>(upsert_workload_id.StatusCode, upsert_workload_id.ApiError.Message, upsert_workload_id.ApiError.Data);
                }
                foreach(var workload_data in workload.Data)
                {
                    var upsert_workload_data_id = await _unitOfWork.WorkloadRepository.CreateUpdateWorkloadAsync(workload_data, upsert_workload_id.Data);
                    if(upsert_workload_data_id.ApiError != null)
                    {
                        return GenerateGenericError.Generate<GenericResponse<bool>, string>(upsert_workload_data_id.StatusCode, upsert_workload_data_id.ApiError.Message, upsert_workload_data_id.ApiError.Data);
                    }

                    await _unitOfWork.WorkloadRepository.DeleteAllWorkloadInformationByWorkloadDataId(upsert_workload_data_id.Data);
                    _unitOfWork.Commit();
                    foreach (var day in workload_data.GetDynamicProperties())
                    {
                        var insert_workload_information = await _unitOfWork.WorkloadRepository.InsertWorkloadInformation(upsert_workload_data_id.Data, day.Key, day.Value);
                        if(insert_workload_information.ApiError != null)
                        {
                            return GenerateGenericError.Generate<GenericResponse<bool>, string>(insert_workload_information.StatusCode, insert_workload_information.ApiError.Message, insert_workload_information.ApiError.Data);
                        }
                    }
                }
                _unitOfWork.Commit();
                return new GenericResponse<bool> { Data = true };
            }
            catch(Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<GenericResponse<bool>>(ex.Message);
            }
        }


    }
}
