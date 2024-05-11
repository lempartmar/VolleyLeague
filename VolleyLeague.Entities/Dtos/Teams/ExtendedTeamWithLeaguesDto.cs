using VolleyLeague.Entities.Dtos.Users;

namespace VolleyLeague.Entities.Dtos.Teams
{
    public class ExtendedTeamWithLeagueDto
    {
        public List<ExtendedTeamDto> ExtendedTeamListDto { get; set; }
        
        public List<LeagueDto> leagueDtos { get; set; }
    }
}
