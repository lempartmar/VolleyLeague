using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Interfaces
{
    public interface ITypedResultService
    {
        Task<List<TypedUserDto>> GetTypedResults(int seasonId);
        Task CreateTypedResult(TypedResultDto typedResult);
    }
}
