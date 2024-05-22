using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Authorize;
using GSRU_API.Services.Implementation;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models.Requests.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController(IEmployeeService _employeeService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly IEmployeeService _employeeService = _employeeService;


        [HttpPost]
        [Route("login")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            var result = await _employeeService.Authorize(request);
            return SetResult(result);
        }

        [HttpPost]
        [Route("login-using-token")]
        [ProducesErrorResponseType(typeof(GenericError<string>))]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        [Authorize(Roles = JwtService.RELOAD_JWT_ROLE)]
        public async Task<IActionResult> LoginUsingToken()
        {
            var employee_id = GetUserId();
            var result = await _employeeService.Authorize(employee_id);
            return SetResult(result);
        }
    }
}
