using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ILeagueService
    {
        Task<List<LeagueDto>> GetAllLeagues();
        Task CreateLeague(LeagueDto league);

        Task<bool> DeleteLeague(int id);

        Task UpdateLeague(LeagueDto league);
    }
}
