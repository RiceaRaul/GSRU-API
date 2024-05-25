using Dapper;
using GSRU_API.Common.Extensions;
using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Employee.Dto;
using GSRU_Common.Models.Teams.Dto;
using GSRU_DataAccessLayer.Common;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GSRU_DataAccessLayer.Repositories
{
    public class EmployeeRepository(IDbTransaction transaction) : RepositoryBase(transaction), IEmployeeRepository
    {
        private const string EMPLOYEE_AUTHORIZE = "employee_authorize";
        private const string EMPLOYEE_AUTHORIZE_BY_ID = "employee_authorize_by_id";

        public async Task<EmployeeDto> Authorize(string username, string password)
        {
            var parameters = new DynamicParameters(new
            {
                username,
                password
            });

            return await AuthorizeInternal(EMPLOYEE_AUTHORIZE, parameters);
        }

        public async Task<EmployeeDto> Authorize(int employeeId)
        {
            var parameters = new DynamicParameters(new
            {
                employee_id = employeeId,
            });

            return await AuthorizeInternal(EMPLOYEE_AUTHORIZE_BY_ID, parameters);
        }


        private async Task<EmployeeDto> AuthorizeInternal(string store_procedure, DynamicParameters parameters)
        {
            try
            {
                var result = await Connection.QueryMultipleAsync(
                  sql: store_procedure,
                  param: parameters,
                  commandType: CommandType.StoredProcedure,
                  commandTimeout: 20,
                  transaction: Transaction
                );
                using (result)
                {
                    var employee = await result.ReadFirstAsync<EmployeeDto>();
                    var roles = await result.ReadAsync<string>();
                    var teams = await result.ReadAsync<TeamDto>();
                    employee.Roles = roles;
                    employee.Teams = teams;
                    return employee;
                }
            }
            catch (SqlException ex) when (ex.Number == (int)CustomSqlException.UserNotExist || ex.Number == (int)CustomSqlException.PasswordIncorrect)
            {
                var enumValue = Enum.Parse<CustomSqlException>(ex.Number.ToString());
                return GenerateGenericError.Generate<EmployeeDto>(HttpStatusCode.Unauthorized, enumValue.ToDescriptionString());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return GenerateGenericError.Generate<EmployeeDto>(HttpStatusCode.NotFound, "EMPLOYEE_NOT_FOUND");
                }
                return GenerateGenericError.GenerateInternalError<EmployeeDto>(ex.Message);
            }
        }
    }
}
