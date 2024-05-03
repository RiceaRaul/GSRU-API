using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Authorize;
using GSRU_API.Services.Interfaces;
using GSRU_Common.Models.Requests.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController(IEmployeeService _employeeService) : BaseController
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
    }
}
