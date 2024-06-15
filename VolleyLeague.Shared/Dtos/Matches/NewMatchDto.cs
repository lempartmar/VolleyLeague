using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class NewMatchDto
    {
        [Display(Name = "Data")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Sezon")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz sezon")]
        public int SeasonId { get; set; }

        [Display(Name = "Liga")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz ligę")]
        public int LeagueId { get; set; }

        [Display(Name = "Miejsce")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz miejsce")]
        public int VenueId { get; set; }

        [Display(Name = "Sektor")]
        public int? Sector { get; set; }

        [Display(Name = "Runda")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz rundę")]
        public int RoundId { get; set; }

        [Display(Name = "Sędzia")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz sędziego")]
        public int RefereeId { get; set; }

        [Display(Name = "Dodatkowe informacje")]
        public string? MatchInfo { get; set; }

        [Display(Name = "Drużyna gospodarzy")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz drużynę gospodarzy")]
        //[Required(ErrorMessage = "Wybierz drużynę gospodarzy")]
        public int? HomeTeamId { get; set; }

        [Display(Name = "Drużyna gości")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz drużynę gości")]
        public int? GuestTeamId { get; set; }

        [Display(Name = "Wynik setu 1 - drużyna gospodarzy")]
        public byte? Set1Team1Score { get; set; }

        [Display(Name = "Wynik setu 2 - drużyna gospodarzy")]
        public byte? Set2Team1Score { get; set; }

        [Display(Name = "Wynik setu 3 - drużyna gospodarzy")]
        public byte? Set3Team1Score { get; set; }

        [Display(Name = "Wynik setu 4 - drużyna gospodarzy")]
        public byte? Set4Team1Score { get; set; }

        [Display(Name = "Wynik setu 5 - drużyna gospodarzy")]
        public byte? Set5Team1Score { get; set; }

        [Display(Name = "Wynik setu 1 - drużyna gości")]
        public byte? Set1Team2Score { get; set; }

        [Display(Name = "Wynik setu 2 - drużyna gości")]
        public byte? Set2Team2Score { get; set; }

        [Display(Name = "Wynik setu 3 - drużyna gości")]
        public byte? Set3Team2Score { get; set; }

        [Display(Name = "Wynik setu 4 - drużyna gości")]
        public byte? Set4Team2Score { get; set; }

        [Display(Name = "Wynik setu 5 - drużyna gości")]
        public byte? Set5Team2Score { get; set; }

        //public static explicit operator Match(NewMatchDto newMatchDto)
        //{
        //    return new Match
        //    {
        //        Schedule = newMatchDto.Schedule,
        //        VenueId = newMatchDto.VenueId,
        //        LeagueId = newMatchDto.LeagueId,
        //        Sector = (byte)newMatchDto.Sector,
        //        RoundId = newMatchDto.RoundId,
        //        RefereeId = newMatchDto.RefereeId,
        //        MatchInfo = newMatchDto.MatchInfo,
        //        HomeTeamId = newMatchDto.HomeTeamId,
        //        GuestTeamId = newMatchDto.GuestTeamId,
        //        MvpId = 1,
        //    };
        //}
    }
}
