using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Services.Interfaces
{
    public interface ILogService
    {
        Task<List<LogDto>> GetLastTenLogs();

        Task AddLog(string description, string link, bool admin, User[] interestedUsers);
    }
}
