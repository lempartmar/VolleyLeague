using Microsoft.AspNetCore.Http;
using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ITeamService
    {
        Task<(bool Success, string Message)> AddTeam(NewTeamDto team, string email);

        Task<TeamDto> GetTeamById(int Id);

        Task<(bool Success, string Message)> DeleteTeamImage(int teamId);

        Task<bool> UpdateNumberOfChangesForAllTeams(int numberOfChanges);

        Task<TeamImage?> GetTeamImageByTeamId(int teamId);

        Task<List<TeamImageDto>> GetAllTeamsImagesStatus();

        Task<(bool Success, string Message)> UploadTeamImage(int teamId, IFormFile file);

        Task<bool> SetAllReportedToPlayToFalse(bool isAccepted);

        Task<List<TeamDto>> GetAllTeams();

        Task<(bool Success, string Message)> UpdateTeam(ManageTeamDto team, string email);

        Task<bool> IsReportedToPlay(int teamId);

        Task<bool> UpdateReportedToPlay(ReportedToPlayDto reportedToPlay);

        Task<bool> UpdateTeamPlayer(PlayerSummaryDto userSummary);

        Task<ManagedTeamDataDto> GetTeamByCaptain(string email);

        Task<ExtendedTeamWithLeagueDto> GetAllExtendedTeams();

        Task<bool> DeleteTeam(int teamId);

        Task<List<TeamDto>> GetTeamsByLeagueId(int leagueId);

        Task<bool> UpdateCaptain(int newCaptainId, string email);

        Task<bool> UpdateExtendedTeam(ExtendedTeamDto extendedTeamDto);

        Task<(bool Success, string Message)> LeaveTeamByEmail(string email);
    }
}