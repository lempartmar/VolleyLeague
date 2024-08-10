using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos.Matches;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Client.Blazor.Services
{

    public interface ITypedResultService
    {
        public Task<List<TypedUserDto>> GetTypedUserDto(int seasonId);

        public Task<bool> CreateTypedResult(TypedResultDto typedResult);

        Task<TypedResultDto?> GetTypedResultByMatchAndUser(int matchId);
        Task<bool> UpdateTypedResult(TypedResultDto typedResultDto);
    }
    public class TypedResultService : ITypedResultService
    {
        private HttpClient _httpClient;

        public TypedResultService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TypedUserDto>> GetTypedUserDto(int seasonId)
        {
            var response = await _httpClient.GetAsync($"api/TypedResult/GetTypedResults?&seasonId={seasonId}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var typeUserResult = JsonSerializer.Deserialize<List<TypedUserDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return typeUserResult;
        }

        public async Task<bool> CreateTypedResult(TypedResultDto typedResult)
        {
            var response = await _httpClient.PostAsJsonAsync("api/typedresult/createtypedresult", typedResult);
            return response.IsSuccessStatusCode;
        }

        public async Task<TypedResultDto?> GetTypedResultByMatchAndUser(int matchId)
        {
            var response = await _httpClient.GetAsync($"api/typedresult/GetTypedResultByMatchAndUser?matchId={matchId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var typedResult = JsonSerializer.Deserialize<TypedResultDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return typedResult;
            }

            return null;
        }

        public async Task<bool> UpdateTypedResult(TypedResultDto typedResultDto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/typedresult/UpdateTypedResult", typedResultDto);
            return response.IsSuccessStatusCode;
        }

    }
}
