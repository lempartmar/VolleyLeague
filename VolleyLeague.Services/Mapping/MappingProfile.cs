using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
            : base()
        {
            CreateMap<League, LeagueDto>()
                  .ReverseMap();
            CreateMap<User, PlayerSummaryDto>();

            CreateMap<Position, PositionDto>()
                  .ReverseMap();

            CreateMap<User, PlayerSummaryDto>();

            CreateMap<Match, MatchSummaryDto>();

            CreateMap<SportsVenue, VenueDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo));

            CreateMap<Match, MatchDto>();

            CreateMap<NewMatchDto, Match>()
                 .ReverseMap();

            CreateMap<User, UserProfileDto>();

            CreateMap<Team, StandingsDto>()
    .AfterMap((team, dto, context) =>
    {
        // Assuming seasonId is passed via context.Items
        var seasonId = (int)context.Items["seasonId"];
        var matches = team.HomeMatches.Concat(team.GuestMatches).Where(m => m.Round.SeasonId == seasonId).ToList();

        dto.MatchesPlayed = matches.Count;
        dto.MatchesWon = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score > m.Team2Score || m.GuestTeamId == team.Id && m.Team2Score > m.Team1Score);
        dto.MatchesLost = dto.MatchesPlayed - dto.MatchesWon;
        dto.SetsWon = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team1Score : m.Team2Score);
        dto.SetsLost = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team2Score : m.Team1Score);
        dto.PointsWon = matches.Sum(m => m.HomeTeamId == team.Id ?
            (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0) :
            (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0));
        dto.PointsLost = matches.Sum(m => m.HomeTeamId == team.Id ?
            (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0) :
            (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0));
        dto.SetsRatio = dto.SetsWon / (double)(dto.SetsLost == 0 ? 1 : dto.SetsLost);
        dto.BallsRatio = dto.PointsWon / (double)(dto.PointsLost == 0 ? 1 : dto.PointsLost);
        dto.Score3_0 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 0 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 0);
        dto.Score3_1 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 1 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 1);
        dto.Score3_2 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 2 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 2);
        dto.Score2_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 2 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 2 && m.Team1Score == 3);
        dto.Score1_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 1 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 1 && m.Team1Score == 3);
        dto.Score0_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 0 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 0 && m.Team1Score == 3);
        dto.Points = dto.MatchesWon * 3 + (team.PointCorrection ?? 0);
    });
        }
    }
}
