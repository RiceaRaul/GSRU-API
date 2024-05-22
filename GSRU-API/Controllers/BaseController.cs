using GSRU_API.Common.Encryption;
using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GSRU_API.Controllers
{
    public class BaseController(IEncryptionService _encryptionService) : ControllerBase
    {
        private readonly IEncryptionService _encryptionService = _encryptionService;
        [NonAction]
        public IActionResult SetResult<T,U>(T data) where T : GenericError<U>
        {
            if(data.ApiError is not null)
            {
                return StatusCode((int)data.StatusCode, data);
            }

            return Ok(data);
        }

        [NonAction]
        public IActionResult SetResult<T>(T data) where T : GenericError<string>
        {
            if (data.ApiError is not null)
            {
                return StatusCode((int)data.StatusCode, data);
            }

            return Ok(data);
        }

        [NonAction]
        public int GetUserId()
        {
            var user = User.Identity as ClaimsIdentity;
            var hashClaim = user?.FindFirst(ClaimTypes.Hash)?.Value;
            if (hashClaim == null)
            {
                return -1;
            }
            var userId = _encryptionService.Decrypt(hashClaim);
            if (int.TryParse(userId, out var id))
            {
                return id;
            }
            return -1;
        }
    }
}
