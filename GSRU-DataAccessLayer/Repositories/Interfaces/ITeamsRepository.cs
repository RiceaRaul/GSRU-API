using GSRU_Common.Models.Teams.Responses;

namespace GSRU_DataAccessLayer.Repositories.Interfaces
{
    public interface ITeamsRepository
    {
        Task<TeamMembersResponse> GetMemberOverView(int team_id);
    }
}
