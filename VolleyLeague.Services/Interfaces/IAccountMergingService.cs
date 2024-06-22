namespace VolleyLeague.Services.Services
{
    public interface IAccountMergingService
    {
        Task<bool> GetHasAccountsForMerging(string email);

        Task<bool> AccountMerging(string email);

        Task<string> GetInfoAboutTheMergedTeam(string email);
    }
}

