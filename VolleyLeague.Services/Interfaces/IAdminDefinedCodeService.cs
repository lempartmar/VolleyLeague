using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Services
{
    public interface IAdminDefinedCodeService
    {
        Task<AdminDefinedCode> GetCodeByKeyAsync(string key);

        Task UpdateCodeAsync(AdminDefinedCode code);
    }
}