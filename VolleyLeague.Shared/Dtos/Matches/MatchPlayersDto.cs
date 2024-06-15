using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class MatchPlayersDto
    {
        public List<PlayerSummaryDto> HomeTeamPlayers { get; set; } = new List<PlayerSummaryDto>();
        public List<PlayerSummaryDto> GuestTeamPlayers { get; set; } = new List<PlayerSummaryDto>();

    }
}
