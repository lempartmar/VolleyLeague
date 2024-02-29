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
        public StandingsDto() { }
        public StandingsDto(Team team, int seasonId)
        {
            Team = (TeamSummaryDto)team;
            var matches = team.HomeMatches.Concat(team.GuestMatches).Where(m => m.Round.SeasonId == seasonId);
            MatchesPlayed = matches.Count();
            MatchesWon = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score > m.Team2Score || m.GuestTeamId == team.Id && m.Team2Score > m.Team1Score);
            MatchesLost = MatchesPlayed - MatchesWon;
            SetsWon = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team1Score : m.Team2Score);
            SetsLost = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team2Score : m.Team1Score);
            PointsWon = matches.Sum(m => m.HomeTeamId == team.Id ?
                (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0) :
                (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0));
            PointsLost = matches.Sum(m => m.HomeTeamId == team.Id ?
                (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0) :
                (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0));
            SetsRatio = (double)SetsWon / (double)(SetsLost == 0 ? 1 : SetsLost);
            BallsRatio = (double)PointsWon / (double)(PointsLost == 0 ? 1 : PointsLost);
            Score3_0 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 0 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 0);
            Score3_1 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 1 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 1);
            Score3_2 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 2 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 2);         
            Score2_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 2 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 2 && m.Team1Score == 3);
            Score1_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 1 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 1 && m.Team1Score == 3);
            Score0_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 0 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 0 && m.Team1Score == 3);
            Points = MatchesWon * 3 + (team.PointCorrection ?? 0);

        }
    }
}
