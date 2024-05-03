using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Entities.Dtos.Matches
{
    public class NewMatchModel : NewMatchDto
    {

        public NewMatchModel()
        {
            SeasonId = 37;
            RoundId = 1237;
            LeagueId = 1;
        }

        private int roundId;
        private int leagueId;
        private int seasonId;

        public int SeasonId
        {
            get => seasonId;
            set
            {
                if (seasonId != value)
                {
                    seasonId = value;
                    UpdateRoundsForSeason();
                }
            }
        }

        public int RoundId
        {
            get => roundId;
            set => roundId = value;
        }

        public int LeagueId
        {
            get => leagueId;
            set
            {
                if(leagueId != value)
                {
                    leagueId = value;
                    UpdateLeagueTeams();
                }
            }
        }

        public DateTime ScheduleDate { get; set; } = DateTime.Today;
        public TimeSpan ScheduleTime { get; set; } = DateTime.Now.TimeOfDay;

        public DateTime Schedule
        {
            get
            {
                return ScheduleDate.Date + ScheduleTime;
            }
            set
            {
                ScheduleDate = value.Date;
                ScheduleTime = value.TimeOfDay;
            }
        }
        public List<LeagueDto> LeagueList { get; set; } = new List<LeagueDto>();
        public List<VenueDto> VenueList { get; set; } = new List<VenueDto>();
        public List<RoundDto> RoundList { get; set; } = new List<RoundDto>();
        public List<RoundDto> SpecificRound { get; set; } = new List<RoundDto>();
        public List<SeasonDto> SeasonList { get; set; } = new List<SeasonDto>();
        public List<PlayerSummaryDto> RefereeList { get; set; } = new List<PlayerSummaryDto>();
        public List<TeamSummaryDto> AllTeamList { get; set; } = new List<TeamSummaryDto>();
        public List<TeamSummaryDto> HomeTeamList { get; set; } = new List<TeamSummaryDto>();
        public List<TeamSummaryDto> GuestTeamList { get; set; } = new List<TeamSummaryDto>();

        private void UpdateRoundsForSeason()
        {
            SpecificRound = RoundList.Where(x => x.SeasonId == seasonId).ToList();
        }

        private void UpdateLeagueTeams()
        {
            HomeTeamList = AllTeamList.Where(t => t.LeagueId == LeagueId).ToList();
            GuestTeamList = AllTeamList.Where(t => t.LeagueId == LeagueId).ToList();
        }
    }
}
