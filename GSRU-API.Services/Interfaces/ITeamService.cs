using GSRU_Common.Models.Teams.Responses;

namespace GSRU_API.Services.Interfaces
{
    public interface ITeamService
    {
        Task<TeamMembersResponse> GetMemberOverView(int team_id);
    }
}
