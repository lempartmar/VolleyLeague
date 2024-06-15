namespace VolleyLeague.Shared.Dtos.Teams
{
    public class ExtendedTeamWithLeagueDto
    {
        public List<ExtendedTeamDto> ExtendedTeamListDto { get; set; }
        
        public List<LeagueDto> leagueDtos { get; set; }
    }
}
