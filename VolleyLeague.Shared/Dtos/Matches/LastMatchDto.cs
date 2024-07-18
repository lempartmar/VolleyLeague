using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class LastMatchDto
    {
        public int Id { get; set; }
        public TeamSummaryDto HomeTeam { get; set; }
        public TeamSummaryDto GuestTeam { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public DateTime Schedule { get; set; }
        public string MatchInfo { get; set; }
    }
}
