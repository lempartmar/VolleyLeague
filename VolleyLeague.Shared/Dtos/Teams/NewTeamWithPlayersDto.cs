namespace VolleyLeague.Shared.Dtos.Teams
{
    public class NewTeamWithPlayersDto : NewTeamDto
    {
        //[ValidateEachItem]
        //[ExcludeFromForm]
        public override List<TeamPlayerDto> Players { get; set; } = new List<TeamPlayerDto>();
    }
}
