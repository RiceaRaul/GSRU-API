using Dapper;
using GSRU_API.Common.Models;
using GSRU_Common.Models.Teams.Dto;
using GSRU_Common.Models.Teams.Responses;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;

namespace GSRU_DataAccessLayer.Repositories
{
    public class TeamsRepository(IDbTransaction transaction) : RepositoryBase(transaction), ITeamsRepository
    {
        private const string TEAMS_GET_MEMBER_OVERVIEW = "teams_get_member_overview";

        public async Task<TeamMembersResponse> GetMemberOverView(int team_id)
        {
            try
            {
                var result = await Connection.QueryAsync<TeamMemberDto>(
                  sql: TEAMS_GET_MEMBER_OVERVIEW,
                  param: new { team_id },
                  commandType: CommandType.StoredProcedure,
                  commandTimeout: 20,
                  transaction: Transaction
                );

                return new TeamMembersResponse
                {
                    TeamMembers = result
                };
            }
            catch (Exception ex)
            {
                return GenerateGenericError.GenerateInternalError<TeamMembersResponse>(ex.Message);
            }
        }
    }
}
