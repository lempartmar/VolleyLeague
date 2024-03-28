using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Entities.Dtos.Matches
{
    public class MatchSummaryDto
    {
        public int Id { get; set; }
        public TeamSummaryDto? HomeTeam { get; set; }
        public TeamSummaryDto? GuestTeam { get; set; }
        public string? LeagueName { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public DateTime Schedule { get; set; }
        public string? VenueName { get; set; }
        public string RoundName { get; set; } = null!;
        public PlayerSummaryDto? Referee { get; set; }
        public PlayerSummaryDto? Mvp { get; set; }
        public string? UnknownRefereeName { get; set; }
        public string? MatchInfo { get; set; }
    }
}
