using Dapper;
using GSRU_API.Common.Extensions;
using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Employee.Dto;
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

        public async Task<EmployeeDto> Authorize(string username, string password)
        {
            try
            {
                var result = await Connection.QueryMultipleAsync(
                  sql: EMPLOYEE_AUTHORIZE,
                  param: new { username, password },
                  commandType: CommandType.StoredProcedure,
                  commandTimeout: 20,
                  transaction: Transaction
                );
                using (result)
                {
                    var employee = await result.ReadFirstAsync<EmployeeDto>();
                    var roles = await result.ReadAsync<string>();
                    employee.Roles = roles;
                    return employee;
                }
            }
            catch(SqlException ex) when (ex.Number == (int)CustomSqlException.UserNotExist || ex.Number == (int)CustomSqlException.PasswordIncorrect)
            {
                var enumValue = Enum.Parse<CustomSqlException>(ex.Number.ToString());
                return GenerateGenericError.Generate<EmployeeDto>(HttpStatusCode.Unauthorized, enumValue.ToDescriptionString());
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<EmployeeDto>(ex.Message);
            }
        }
    }
}
