using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Entities.Dtos.Matches
{ 
    public class MatchPlayersDto
    {
        public List<PlayerSummaryDto> HomeTeamPlayers { get; set; } = new List<PlayerSummaryDto>();
        public List<PlayerSummaryDto> GuestTeamPlayers { get; set; } = new List<PlayerSummaryDto>();

    }
}
