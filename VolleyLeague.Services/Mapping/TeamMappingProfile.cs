using AutoMapper;
using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Mapping
{
    internal class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
            : base()
        {
            CreateMap<Team, ExtendedTeamDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsReportedToPlay, opt => opt.MapFrom(src => src.IsReportedToPlay))
                .ForMember(dest => dest.Accepted, opt => opt.MapFrom(src => src.Accepted))
                .ForMember(dest => dest.LeagueName, opt => opt.MapFrom(src => src.League.Name))
                .ForMember(dest => dest.LeagueId, opt => opt.MapFrom(src => src.League.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ChangeCount, opt => opt.MapFrom(src => src.ChangeCount))
                .ForMember(dest => dest.PointCorrection, opt => opt.MapFrom(src => src.PointCorrection));

            CreateMap<ExtendedTeamDto, Team>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsReportedToPlay, opt => opt.MapFrom(src => src.IsReportedToPlay))
                .ForMember(dest => dest.Accepted, opt => opt.MapFrom(src => src.Accepted))
                .ForMember(dest => dest.LeagueId, opt => opt.MapFrom(src => src.LeagueId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ChangeCount, opt => opt.MapFrom(src => src.ChangeCount))
                .ForMember(dest => dest.PointCorrection, opt => opt.MapFrom(src => src.PointCorrection));

            CreateMap<Team, TeamDto>()
             .ForMember(dest => dest.League, opt => opt.MapFrom(src => src.League))
             .ForMember(dest => dest.Captain, opt => opt.MapFrom(src => src.Captain))
             .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.TeamPlayers.Select(tp => tp.Player)))
             .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Image));

            CreateMap<User, TeamPlayerDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AdditionalEmail))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<TeamPlayer, TeamPlayerDto>()
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Player.Position.Name))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Player.Position.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Player.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Player.LastName));

            //CreateMap<ManageTeamDto, Team>()
            //.ForMember(dest => dest.TeamPlayers, opt => opt.Ignore());

            CreateMap<TeamPlayerDto, User>();
            CreateMap<TeamPlayerDto, User>();

            CreateMap<PlayerSummaryDto, User>()
             .ForMember(dest => dest.JerseyNumber, opt => opt.MapFrom(src => (byte?)src.JerseyNumber))
             .ForMember(dest => dest.Height, opt => opt.MapFrom(src => (byte?)src.Height))
             .ForMember(dest => dest.AdditionalEmail, opt => opt.MapFrom(src => src.AdditionalEmail))
             .ForMember(dest => dest.Position, opt => opt.Ignore());

            CreateMap<TeamPlayerDto, User>()
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Height, opt => opt.MapFrom(src => (byte?)src.Height))
             .ForMember(dest => dest.JerseyNumber, opt => opt.MapFrom(src => (byte?)src.JerseyNumber))
             .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
             .ForMember(dest => dest.Credentials, opt => opt.Ignore());

            CreateMap<User, TeamPlayerShortDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name));

            CreateMap<TeamPlayerDto, TeamPlayer>()
             .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src))
             .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<User, TeamPlayer>()
             .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src))
             .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<TeamPlayer, TeamPlayerDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Player.AdditionalEmail))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Player.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Player.LastName))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Player.PositionId))
            .ForMember(dest => dest.JerseyNumber, opt => opt.MapFrom(src => src.Player.JerseyNumber))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Player.Gender))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Player.Height));



            CreateMap<Team, ManagedTeamDataDto>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.TeamPlayers));
            CreateMap<ManagedTeamDataDto, Team>();

            CreateMap<NewTeamDto, Team>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
             .ForMember(dest => dest.TeamDescription, opt => opt.MapFrom(src => src.TeamDescription))
             .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
             .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo))
             .ForMember(dest => dest.TeamPlayers, opt => opt.Ignore())
             .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.LeagueId, opt => opt.Ignore())
             .ForMember(dest => dest.Accepted, opt => opt.Ignore())
             .ForMember(dest => dest.CaptainId, opt => opt.Ignore())
             .ForMember(dest => dest.IsReportedToPlay, opt => opt.Ignore())
             .ForMember(dest => dest.ChangeCount, opt => opt.Ignore())
             .ForMember(dest => dest.ImageWidth, opt => opt.Ignore())
             .ForMember(dest => dest.ImageHeight, opt => opt.Ignore())
             .ForMember(dest => dest.LogoWidth, opt => opt.Ignore())
             .ForMember(dest => dest.LogoHeight, opt => opt.Ignore())
             .ForMember(dest => dest.PointCorrection, opt => opt.Ignore())
             .ForMember(dest => dest.Captain, opt => opt.Ignore())
             .ForMember(dest => dest.League, opt => opt.Ignore())
             .ForMember(dest => dest.HomeMatches, opt => opt.Ignore())
             .ForMember(dest => dest.GuestMatches, opt => opt.Ignore())
             .ForMember(dest => dest.Invitations, opt => opt.Ignore());
        }
    }
}
