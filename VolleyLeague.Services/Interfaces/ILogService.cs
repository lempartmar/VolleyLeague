using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Interfaces
{
    public interface ILogService
    {
        Task<List<LogDto>> GetLastTenLogs();

        Task AddLog(string description, string link, bool admin, User[] interestedUsers);
    }
}
