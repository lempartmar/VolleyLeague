using VolleyLeague.Shared.Dtos.Matches;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ITypedResultService
    {
        Task<List<TypedUserDto>> GetTypedResults(int seasonId);
        Task CreateTypedResult(TypedResultDto typedResult, string identity);

        Task<TypedResultDto?> GetTypedResultByMatchAndUserAsync(int matchId, string identity);

        Task<bool> UpdateTypedResultAsync(TypedResultDto typedResultDto, string identity);
    }
}
