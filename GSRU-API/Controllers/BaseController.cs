using GSRU_API.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    public class BaseController : ControllerBase
    {

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
    }
}
