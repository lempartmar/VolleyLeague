using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Entities.Dtos.Matches
{
    public class StandingsDto
    {
        public TeamSummaryDto Team { get; set; } = null!;
        public int Points { get; set; } = 0;
        public int MatchesPlayed { get; set; } = 0;
        public int MatchesWon { get; set; } = 0;
        public int MatchesLost { get; set; } = 0;
        public int SetsWon { get; set; } = 0;
        public int SetsLost { get; set; } = 0;
        public int PointsWon { get; set; } = 0;
        public int PointsLost { get; set; } = 0;
        public double SetsRatio { get; set; } = 0;
        public double BallsRatio { get; set; } = 0;
        public int Score3_0 { get; set; } = 0;
        public int Score3_1 { get; set; } = 0;
        public int Score3_2 { get; set; } = 0;
        public int Score2_3 { get; set; } = 0;
        public int Score1_3 { get; set; } = 0;
        public int Score0_3 { get; set; } = 0;
    }
}
