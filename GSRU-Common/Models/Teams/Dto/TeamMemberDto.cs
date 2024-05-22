using GSRU_API.Common.Models;

namespace GSRU_Common.Models.Teams.Dto
{
    public class TeamMemberDto
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public bool IsLeader { get; init; }
    }
}
