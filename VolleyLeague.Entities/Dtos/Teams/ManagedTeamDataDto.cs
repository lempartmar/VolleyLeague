namespace VolleyLeague.Entities.Dtos.Teams
{
    public class ManagedTeamDataDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public TeamPlayerDto Captain { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public LeagueDto? League { get; set; }

        public List<TeamPlayerDto> Players { get; set; } = null!;

        public string Email { get; set; } = null!;

        public byte[]? Image { get; set; }

        public byte[]? Logo { get; set; }

        public byte[]? Photo { get; set; }

        public string Phone { get; set; } = null!;

        public string? TeamDescription { get; set; }

        public string? Website { get; set; }

        public int? AvailableTransfers { get; set; }

        // Method to generate TeamDto from Team


    }
}