using GSRU_API.Common.Models.Authorize;
using GSRU_Common.Models.Requests.Employee;

namespace GSRU_API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<AuthenticationResponse> Authorize(LoginModel request);
    }
}
