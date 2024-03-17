using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Entities.Dtos.Matches
{
    public class NewMatchModel : NewMatchDto
    {
        public List<LeagueDto> LeagueList { get; set; } = new List<LeagueDto>() { };
        public List<VenueDto> VenueList { get; set; } = new List<VenueDto>() { };

        public List<RoundDto> RoundList { get; set; } = new List<RoundDto>() { };

        public List<SeasonDto> SeasonList { get; set; } = new List<SeasonDto>() { };

        public List<PlayerSummaryDto> RefereeList { get; set; } = new List<PlayerSummaryDto>() { };

        public List<TeamSummaryDto> HomeTeamList { get; set; } = new List<TeamSummaryDto>() { };

        public List<TeamSummaryDto> GuestTeamList { get; set; } = new List<TeamSummaryDto>() { };
    }
}
