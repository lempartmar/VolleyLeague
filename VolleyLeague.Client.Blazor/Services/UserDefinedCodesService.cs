using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos;
using VolleyLeague.Shared.Dtos.Discussion;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IUserDefinedCodeService
    {
        Task<string> GetValueByKey(string key);

        Task<(bool Success, string Message)> UpdateCode(UserDefinedCodeDto code);
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
            var value = result.Value;
            return value;
        }

        public async Task<(bool Success, string Message)> UpdateCode(UserDefinedCodeDto code)
        {
            var response = await _httpClient.PutAsJsonAsync("api/UserDefinedCode/UpdateCode", code);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, content);
            }
            else
            {
                return (false, content);
            }
        }
    }
}
