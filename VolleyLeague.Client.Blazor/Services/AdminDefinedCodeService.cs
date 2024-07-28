using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IAdminDefinedCodeService
    {
        Task<string> GetValueByKey(string key);

        Task<(bool Success, string Message)> UpdateCode(AdminDefinedCodeDto code);
    }
    public class AdminDefinedCodeService : IAdminDefinedCodeService
    {
        private readonly HttpClient _httpClient;

        public AdminDefinedCodeService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<string> GetValueByKey(string key)
        {
            var response = await _httpClient.GetAsync($"api/AdminDefinedCode/GetCodeByKey/{key}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AdminDefinedCodeDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var value = result.Value;
            return value;
        }

        public async Task<(bool Success, string Message)> UpdateCode(AdminDefinedCodeDto code)
        {
            var response = await _httpClient.PutAsJsonAsync("api/AdminDefinedCode/UpdateCode", code);
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
