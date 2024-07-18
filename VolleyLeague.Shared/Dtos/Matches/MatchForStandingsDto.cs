using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class MatchForStandingsDto
    {
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public byte Team1Score { get; set; }
        public byte Team2Score { get; set; }
        public byte? Set1Team1Score { get; set; }
        public byte? Set1Team2Score { get; set; }
        public byte? Set2Team1Score { get; set; }
        public byte? Set2Team2Score { get; set; }
        public byte? Set3Team1Score { get; set; }
        public byte? Set3Team2Score { get; set; }
        public byte? Set4Team1Score { get; set; }
        public byte? Set4Team2Score { get; set; }
        public byte? Set5Team1Score { get; set; }
        public byte? Set5Team2Score { get; set; }
        public TeamSummaryDto HomeTeam { get; set; }
        public TeamSummaryDto GuestTeam { get; set; }
    }
}
