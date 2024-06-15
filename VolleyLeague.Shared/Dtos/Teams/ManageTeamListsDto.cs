namespace VolleyLeague.Shared.Dtos.Teams
{
    public class ManageTeamListsDto : ManageTeamDto
    {
        public override List<TeamPlayerDto> Players { get; set; } = new List<TeamPlayerDto>();
        public override List<TeamPlayerDto> NewPlayers { get; set; } = new List<TeamPlayerDto>();
        public override List<TeamPlayerDto> RemovedPlayers { get; set; } = new List<TeamPlayerDto>();
        public override TeamPlayerDto Captain { get; set; } = new TeamPlayerDto();

        public ManageTeamListsDto(ManageTeamDto manageTeam)
        {
            this.Email = manageTeam.Email;
            this.Logo = manageTeam.Logo;
            this.Photo = manageTeam.Photo;
            this.Phone = manageTeam.Phone;
            this.Players = manageTeam.Players;
            this.TeamDescription = manageTeam.TeamDescription;
            this.Website = manageTeam.Website;
            this.Captain = manageTeam.Captain;
        }

        public ManageTeamListsDto()
        {
        }
    }
}
