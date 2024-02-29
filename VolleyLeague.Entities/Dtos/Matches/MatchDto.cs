using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Entities.Dtos.Matches
{

    public partial class MatchDto
    {
        public int Id { get; set; }

        public DateTime Schedule { get; set; }

        public DateTime CreationDate { get; set; }

        public string? VenueName { get; set; }

        public VenueDto? Venue { get; set; }

        public LeagueDto League { get; set; } = null!;

        public int Sector { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public string? RoundName { get; set; }

        public PlayerSummaryDto? Referee { get; set; }

        public string? UnknownRefereeName { get; set; }

        public string? MatchInfo { get; set; }

        public PlayerSummaryDto? Mvp { get; set; }

        public int? Set1Team1Score { get; set; }

        public int? Set2Team1Score { get; set; }

        public int? Set3Team1Score { get; set; }

        public int? Set4Team1Score { get; set; }

        public int? Set5Team1Score { get; set; }

        public int? Set1Team2Score { get; set; }

        public int? Set2Team2Score { get; set; }

        public int? Set3Team2Score { get; set; }

        public int? Set4Team2Score { get; set; }

        public int? Set5Team2Score { get; set; }

        public string? MatchLeague { get; set; }

        public TeamDto HomeTeam { get; set; } = null!;

        public TeamDto GuestTeam { get; set; } = null!;

        }
    }

