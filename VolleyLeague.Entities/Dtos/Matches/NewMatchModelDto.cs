using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Dtos.Teams;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [Required(ErrorMessage = "Sezon jest wymagany.")]
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

        [Required(ErrorMessage = "Runda jest wymagana.")]
        public int RoundId
        {
            get => roundId;
            set => roundId = value;
        }

        [Required(ErrorMessage = "Liga jest wymagana.")]
        public int LeagueId
        {
            get => leagueId;
            set
            {
                if (leagueId != value)
                {
                    leagueId = value;
                    UpdateLeagueTeams();
                }
            }
        }

        [Required(ErrorMessage = "Data i czas są wymagane.")]
        public DateTime Schedule { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Miejsce meczu jest wymagane.")]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Sędzia jest wymagany.")]
        public int RefereeId { get; set; }

        [Required(ErrorMessage = "Drużyna gospodarzy jest wymagana.")]
        public int HomeTeamId { get; set; }

        [Required(ErrorMessage = "Drużyna gości jest wymagana.")]
        public int GuestTeamId { get; set; }

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
