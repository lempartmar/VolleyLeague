namespace VolleyLeague.Entities.Models
{
    public partial class Team : BaseEntity
    {
        public string Name { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public byte[]? Image { get; set; }

        public int LeagueId { get; set; }

        public bool Accepted { get; set; }

        public int CaptainId { get; set; }

        public string Email { get; set; } = null!;

        public bool? IsReportedToPlay { get; set; }

        public int ChangeCount { get; set; }

        public int? ImageWidth { get; set; }

        public int? ImageHeight { get; set; }

        public byte[]? Logo { get; set; }

        public int? LogoWidth { get; set; }

        public int? LogoHeight { get; set; }

        public string Phone { get; set; } = null!;

        public string? TeamDescription { get; set; }

        public string? Website { get; set; }

        public int? PointCorrection { get; set; }

        public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

        public virtual User Captain { get; set; } = null!;

        public virtual League League { get; set; } = null!;


        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> GuestMatches { get; set; } = new List<Match>();

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}