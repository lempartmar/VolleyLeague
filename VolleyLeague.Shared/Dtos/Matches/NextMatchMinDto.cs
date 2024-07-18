using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class NextMatchMinDto
    {
        public int Id { get; set; }
        public TeamSummaryDto? HomeTeam { get; set; }
        public TeamSummaryDto? GuestTeam { get; set; }
        public DateTime Schedule { get; set; }
    }
}
