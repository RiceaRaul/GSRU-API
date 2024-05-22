using GSRU_API.Attributes;
using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController(ITeamService _teamService, IEncryptionService encryptionService) : BaseController(encryptionService)
    {
        private readonly ITeamService _teamService = _teamService;

        [HttpGet("get-member-overview/{team_id:int}")]
        [Authorize]
        /*[HaveRole("VIEW_TEAM_","team_id")]*/
        public async Task<IActionResult> GetMemberOverView(int team_id)
        {
            var response = await _teamService.GetMemberOverView(team_id);
            return SetResult(response);
        }
    }

    public class TeamRequest
    {
        public int TeamId { get; set; }
    }
}
