using GSRU_API.Common.Models.Employee.Dto;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> Authorize(string username, string password);
    }
}
