using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Authorize;
using GSRU_API.Common.Models.Employee.Dto;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models.Requests.Employee;
using GSRU_DataAccessLayer.Interfaces;
using System.Security.Claims;

namespace GSRU_API.Services.Implementation
{
    public class EmployeeService(IUnitOfWork _unitOfWork, IJwtService _jwtService, IEncryptionService _encryptionService) : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;
        private readonly IJwtService _jwtService = _jwtService;
        private readonly IEncryptionService _encryptionService = _encryptionService;

        public async Task<AuthenticationResponse> Authorize(LoginModel request)
        {
            var passwordEncrypted = _encryptionService.Encrypt(request.Password);
            var employee = await _unitOfWork.EmployeeRepository.Authorize(request.UserName, passwordEncrypted);
            if(employee.ApiError is not null)
            {
                return GenerateGenericError.Generate<AuthenticationResponse>(employee.StatusCode, employee.ApiError.Message,employee.ApiError.Data);
            }
            var claims = GetClaims(employee);
            var token = _jwtService.CreateJwt(claims, employee.CompanyEmail);

            return token;
        }

        private Claim[] GetClaims(EmployeeDto employee)
        {
            List<Claim> claims = [
                new Claim(ClaimTypes.Name, employee.CompanyEmail),
                new Claim(ClaimTypes.Hash, _encryptionService.Encrypt(employee.Id.ToString() ?? ""))
            ];
            foreach (var role in employee.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return [.. claims];
        }
    }
}
