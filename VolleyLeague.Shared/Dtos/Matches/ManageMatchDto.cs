namespace VolleyLeague.Shared.Dtos.Matches
{
    public partial class ManageMatchDto
    {
        public int Id { get; set; }

        public DateTime Schedule { get; set; }

        public int? VenueId { get; set; }

        public int Sector { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public int? RefereeId { get; set; }

        public string? UnknownRefereeName { get; set; }

        public string? MatchInfo { get; set; }

        public int? MvpId { get; set; }

        public int? Set1Team1Score { get; set; }

        public int? Set2Team1Score { get; set; }

        public int? Set3Team1Score { get; set; }

        public int? Set4Team1Score { get; set; }

        public int? Set5Team1Score { get; set; }

        public int? Set1Team2Score { get; set; }

        public int? Set2Team2Score { get; set; }

        public int? Set3Team2Score { get; set; }

        public int? Set4Team2Score { get; set; }

        public int? Set5Team2Score { get; set; }

        public string? MatchLeague { get; set; }

        public static explicit operator ManageMatchDto(MatchDto matchDto)
        {
            return new ManageMatchDto
            {
                Id = matchDto.Id,
                Schedule = matchDto.Schedule,
                VenueId = matchDto.Venue?.Id,
                Sector = matchDto.Sector,
                Team1Score = matchDto.Team1Score,
                Team2Score = matchDto.Team2Score,
                RefereeId = matchDto.Referee?.Id,
                UnknownRefereeName = matchDto.UnknownRefereeName,
                MatchInfo = matchDto.MatchInfo,
                MvpId = matchDto.Mvp?.Id,
                Set1Team1Score = matchDto.Set1Team1Score,
                Set2Team1Score = matchDto.Set2Team1Score,
                Set3Team1Score = matchDto.Set3Team1Score,
                Set4Team1Score = matchDto.Set4Team1Score,
                Set5Team1Score = matchDto.Set5Team1Score,
                Set1Team2Score = matchDto.Set1Team2Score,
                Set2Team2Score = matchDto.Set2Team2Score,
                Set3Team2Score = matchDto.Set3Team2Score,
                Set4Team2Score = matchDto.Set4Team2Score,
                Set5Team2Score = matchDto.Set5Team2Score,
                MatchLeague = matchDto.MatchLeague
            };
        }
    }
}
