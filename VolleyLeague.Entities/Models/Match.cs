namespace VolleyLeague.Entities.Models
{
    public partial class Match : BaseEntity
    {
        public DateTime Schedule { get; set; }

        public DateTime CreationDate { get; set; }

        public int VenueId { get; set; }

        public int LeagueId { get; set; }

        public byte Sector { get; set; }

        public byte Team1Score { get; set; }

        public byte Team2Score { get; set; }

        public int RoundId { get; set; }

        public int? RefereeId { get; set; }

        public string? UnknownRefereeName { get; set; }

        public string? MatchInfo { get; set; }

        public int MvpId { get; set; }

        public byte? Set1Team1Score { get; set; }

        public byte? Set2Team1Score { get; set; }

        public byte? Set3Team1Score { get; set; }

        public byte? Set4Team1Score { get; set; }

        public byte? Set5Team1Score { get; set; }

        public byte? Set1Team2Score { get; set; }

        public byte? Set2Team2Score { get; set; }

        public byte? Set3Team2Score { get; set; }

        public byte? Set4Team2Score { get; set; }

        public byte? Set5Team2Score { get; set; }

        public string? MatchLeague { get; set; }

        public int HomeTeamId { get; set; }

        public int GuestTeamId { get; set; }
        public virtual Team HomeTeam { get; set; } = null!;

        public virtual Team GuestTeam { get; set; } = null!;

        public virtual Round Round { get; set; } = null!;

        public virtual League League { get; set; } = null!;

        public virtual SportsVenue Venue { get; set; } = null!;

        public virtual User Mvp { get; set; } = null!;

        public virtual User? Referee { get; set; }

        public virtual ICollection<TypedResult> TypedResults { get; set; } = new List<TypedResult>();
    }
}