using GSRU_API.Common.Encryption.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController(IEncryptionService encryptionService) : ControllerBase
    {
        [HttpGet("Encrypt")]
        public IActionResult Encrypt([FromQuery] string text)
        {
            return Ok(encryptionService.Encrypt(text));
        }

        [HttpGet("Decrypt")]
        public IActionResult Decrypt([FromQuery] string text)
        {
            return Ok(encryptionService.Decrypt(text));
        }
    }
}
