using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public interface IAccountMergingService
    {
        Task<TeamsToMergeDto> GetHasAccountsForMerging(string email);

        Task<bool> AccountMerging(string email);

        Task<string> GetInfoAboutTheMergedTeam(string email);
    }
}

