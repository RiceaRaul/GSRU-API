using GSRU_Common.Encryption;
using GSRU_Common.Encryption.Interfaces;
using GSRU_DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController(IUnitOfWork _unitOfWork, IEncryptionService encryptionService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Register(string data)
        {
            
            var test = encryptionService.Encrypt(data);
            return Ok(test);
        }
/*        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
                return BadRequest("User does not exist");

            if (!await _userManager.CheckPasswordAsync(user, user.Password))
                return BadRequest("Invalid password");

            var token = await GenerateJwtToken(user);
            return Ok(token);
        }*/
    }
}
