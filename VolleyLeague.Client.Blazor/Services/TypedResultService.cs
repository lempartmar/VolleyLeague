using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Dtos;
using VolleyLeague.Entities.Dtos.Matches;

namespace VolleyLeague.Client.Blazor.Services
{

    public interface ITypedResultService
    {
        public Task<List<TypedUserDto>> GetTypedUserDto(int seasonId);

        public Task<bool> CreateTypedResult(TypedResultDto typedResult);
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
    }
}
