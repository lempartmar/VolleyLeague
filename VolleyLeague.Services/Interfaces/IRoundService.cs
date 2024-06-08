using VolleyLeague.Entities.Dtos.Matches;

namespace VolleyLeague.Services.Interfaces
{
    public interface IRoundService
    {
        Task<List<RoundDto>> GetAllRounds();

        Task CreateRound(RoundDto round);

        Task UpdateRound(RoundDto round);

        Task<bool> DeletePosition(int id);

        Task<List<RoundDto>> GetRoundsBySeasonId(int? seasonId);
    }
}
