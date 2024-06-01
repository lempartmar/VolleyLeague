using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface IMatchService
    {
        Task<List<MatchSummaryDto>> GetAllMatchesAsync();

        Task<bool> DeleteMatch(int id);

        Task<MatchDto> GetMatchByIdAsync(int id);

        Task<List<PlayerSummaryDto>> GetReferees();

        Task<List<PlayerSummaryDto>> GetPotentialReferees();

        Task<bool> RemoveReferee(int userId);

        Task<List<PlayerSummaryDto>> GetOtherData();

        Task<List<MatchSummaryDto>> GetMatchesByLeagueIdAsync(int id);

        Task<List<MatchSummaryDto>> GetMatches(int seasonId, int teamId);

        Task<List<MatchSummaryDto>> GetMatches(int leagueId, int seasonId, int roundId);

        Task AddMatch(NewMatchDto match);

        Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId);

        Task<List<MatchSummaryDto>> GetLast10Matches();

        Task<bool> AddReferee(int userId);

        Task<List<PlayerSummaryDto>> GetMvpBySeasonAndLeague(int seasonId, int leagueId);
    }
}
