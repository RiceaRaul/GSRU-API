using GSRU_API.Services.Interfaces;
using GSRU_Common.Models.Teams.Responses;
using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_API.Services.Implementation
{
    public class TeamService(IUnitOfWork _unitOfWork) : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;

        public async Task<TeamMembersResponse> GetMemberOverView(int team_id)
        {
            var result = await  _unitOfWork.TeamsRepository.GetMemberOverView(team_id);
            return result;
        }
    }
}
