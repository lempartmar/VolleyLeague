using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IUserDefinedCodeService
    {
        Task<string> GetValueByKey(string key);
    }
    public class UserDefinedCodeService : IUserDefinedCodeService
    {
        private readonly HttpClient _httpClient;

        public UserDefinedCodeService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<string> GetValueByKey(string key)
        {
            var response = await _httpClient.GetAsync($"api/UserDefinedCode/GetCodeByKey/{key}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserDefinedCodeDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result.Value;
        }
    }
}
