using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Services
{
    public interface IUserDefinedCodeService
    {
        Task<UserDefinedCode> GetCodeByKeyAsync(string key);

        Task UpdateCodeAsync(UserDefinedCode code);
    }
}