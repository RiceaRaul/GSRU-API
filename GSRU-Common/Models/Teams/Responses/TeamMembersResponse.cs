using GSRU_API.Common.Models;
using GSRU_Common.Models.Teams.Dto;

namespace GSRU_Common.Models.Teams.Responses
{
    public class TeamMembersResponse : GenericError<string>
    {
        public IEnumerable<TeamMemberDto> TeamMembers { get; init; } = Enumerable.Empty<TeamMemberDto>();
    }
}
