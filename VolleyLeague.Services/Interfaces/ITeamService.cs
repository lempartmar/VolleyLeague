using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ITeamService
    {
        Task AddTeam(NewTeamDto team);

        Task<TeamDto> GetTeamById(int Id);

        Task<List<TeamDto>> GetAllTeams();

        Task<bool> UpdateTeam(ManageTeamDto teamDto);

        Task<bool> UpdateTeamPlayer(PlayerSummaryDto userSummary);

        Task<ManagedTeamDataDto> GetTeamByCaptain(string email);

        Task<ExtendedTeamWithLeagueDto> GetAllExtendedTeams();

        Task<List<TeamDto>> GetTeamsByLeagueId(int leagueId);

        Task<bool> UpdateCaptain(int newCaptainId, string email);

        Task<bool> UpdateExtendedTeam(ExtendedTeamDto extendedTeamDto);
    }
}