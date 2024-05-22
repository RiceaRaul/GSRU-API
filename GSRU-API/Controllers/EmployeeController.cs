using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models.Employee.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController(IEmployeeService _employeeService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly IEmployeeService _employeeService = _employeeService;

        [HttpGet("get-employee/{id:int?}")]
        [ProducesResponseType(typeof(EmployeeData), 200)]
        public async Task<IActionResult> GetEmployee([SwaggerParameter(Required = false)]  int? id = null)
        {
            var employee_id = GetUserId();
            var response = await _employeeService.GetEmployeeDataById(id ?? employee_id);
            return SetResult(response);
        }
    }
}
