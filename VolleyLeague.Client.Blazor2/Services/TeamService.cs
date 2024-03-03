using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Client.Blazor2.Services
{ 
    public interface ITeamService
    {
        public Task<List<TeamSummaryDto>> GetAllTeams();
        public Task<TeamDto> GetTeam(int id);
        public Task<bool> CreateTeam(NewTeamDto team);
        public Task<bool> UpdateTeam(ManageTeamDto team);
        public Task<bool> DeleteTeam(int id);
        public Task<List<TeamDto>> GetTeamsByLeague(int leagueId);
        public Task<ManagedTeamDataDto> GetManagedTeam();
        public Task<List<LeagueDto>> GetLeagues();
        public Task<bool> UpdateCaptain(int playerId);

    }

    public class TeamService : ITeamService
    {
        private HttpClient _httpClient;

        public TeamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<List<TeamSummaryDto>> GetAllTeams()
        {
            var response = await _httpClient.GetAsync("api/team");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TeamSummaryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TeamDto> GetTeam(int id)
        {
            var response = await _httpClient.GetAsync($"api/team/GetTeamById/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TeamDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ManagedTeamDataDto> GetManagedTeam()
        {
            var response = await _httpClient.GetAsync($"api/team/managedteam");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ManagedTeamDataDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<TeamDto>> GetTeamsByLeague(int leagueId)
        {
            var response = await _httpClient.GetAsync($"api/team/GetTeamsByLeagueId/{leagueId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TeamDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> CreateTeam(NewTeamDto team)
        {
            var response = await _httpClient.PostAsJsonAsync("api/team", team);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTeam(ManageTeamDto team)
        {
            var response = await _httpClient.PutAsJsonAsync("api/team", team);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTeam(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/team/id/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCaptain(int playerId)
        {
            var response = await _httpClient.PutAsJsonAsync("api/team/updateCaptain", new { playerId });
            return response.IsSuccessStatusCode;
        }
        public async Task<List<LeagueDto>> GetLeagues()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/League/GetAllLeagues");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<LeagueDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                // Log error, throw exception, or handle error as needed
                throw new Exception("An error occurred while fetching leagues.");
            }
        }
    }
}
