using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface ITeamService
    {
        public Task<List<TeamSummaryDto>> GetAllTeams();
        public Task<List<TeamDto>> GetAllTeamsDto();
        public Task<(bool Success, string Message)> UpdateNumberOfChangesForAllTeams(int numberOfChanges);
        public Task<TeamDto> GetTeam(int id);
        public Task<(bool Success, string Message)> CreateTeam(NewTeamDto team);
        Task<(bool Success, string Message)> UpdateTeam(ManageTeamDto team);
        Task<(bool Success, string Message)> UpdateReportedToPlay(ReportedToPlayDto reportedToPlay);
        Task<bool> IsReportedToPlay(int teamId);
        public Task<bool> DeleteTeam(int id);
        public Task<List<TeamDto>> GetTeamsByLeague(int leagueId);
        public Task<(bool Success, string Message)> SetAllReportedToPlayToFalse(bool isAccepted);
        public Task<ManagedTeamDataDto> GetManagedTeam();
        public Task<List<LeagueDto>> GetLeagues();
        public Task<bool> UpdateCaptain(int newCaptainEmail);
        public Task<ExtendedTeamWithLeagueDto> GetAllTeamsForEdit();
        public Task<bool> UpdateExtendedTeamByAdmin(ExtendedTeamDto team);
        public Task<List<TeamImageDto>> GetAllTeamsImageStatus();
        public Task<TeamsToMergeDto> GetHasAccountsForMerging(string email);
        public Task<bool> AccountMerging(string email);
        public Task<string> GetInfoAboutTeamsToMerge(string email);
        public Task<(bool Success, string Message)> LeaveTeamByEmail(string email);
    }

    public class TeamService : ITeamService
    {
        private HttpClient _httpClient;

        public TeamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TeamDto>> GetAllTeamsDto()
        {
            var response = await _httpClient.GetAsync("api/team/getallteams");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TeamDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<TeamImageDto>> GetAllTeamsImageStatus()
        {
            var response = await _httpClient.GetAsync("api/team/getallteamsimagesstatus");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TeamImageDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<TeamSummaryDto>> GetAllTeams()
        {
            var response = await _httpClient.GetAsync("api/team/getallteams");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TeamSummaryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ExtendedTeamWithLeagueDto> GetAllTeamsForEdit()
        {
            var response = await _httpClient.GetAsync("api/team/getallteamsforedit");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExtendedTeamWithLeagueDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
            var response = await _httpClient.GetAsync($"api/team/GetManagedTeam");
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

        public async Task<(bool Success, string Message)> CreateTeam(NewTeamDto team)
        {
            var response = await _httpClient.PostAsJsonAsync("api/team/createteam", team);
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

        public async Task<(bool Success, string Message)> UpdateTeam(ManageTeamDto team)
        {
            var response = await _httpClient.PutAsJsonAsync("api/team/UpdateTeam", team);
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

        public async Task<bool> UpdateExtendedTeamByAdmin(ExtendedTeamDto team)
        {
            var response = await _httpClient.PutAsJsonAsync("api/team/updateextendedteam", team);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Success, string Message)> SetAllReportedToPlayToFalse(bool isAccepted)
        {
            var response = await _httpClient.PutAsync($"api/team/SetAllReportedToPlayToFalse?isAccepted={isAccepted}", null);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return (true, content);
            }
            else
            {
                var result = JsonSerializer.Deserialize<JsonResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (false, result.Message);
            }
        }

        public async Task<bool> DeleteTeam(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/team/deleteteam/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCaptain(int newCaptainId)
        {
            var response = await _httpClient.PutAsync($"api/team/updateCaptain?newCaptainId={newCaptainId}", null);
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
                throw new Exception("An error occurred while fetching leagues.");
            }
        }

        public async Task<TeamsToMergeDto> GetHasAccountsForMerging(string email)
        {
            var response = await _httpClient.GetAsync($"api/AccountMerging/GetHasAccountsForMerging?email={email}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TeamsToMergeDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> AccountMerging(string email)
        {
            var response = await _httpClient.DeleteAsync($"api/AccountMerging/AccountMerging?email={email}");
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetInfoAboutTeamsToMerge(string email)
        {
            var response = await _httpClient.GetAsync($"api/AccountMerging/GetInfoAboutTeamsToMerge?email={email}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var teamName = JsonSerializer.Deserialize<string>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return teamName;
        }

        public async Task<bool> IsReportedToPlay(int teamId)
        {
            var response = await _httpClient.GetAsync($"api/team/IsReportedToPlay/{teamId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result.Success;
            }
            return false;
        }

        public async Task<(bool Success, string Message)> UpdateNumberOfChangesForAllTeams(int numberOfChanges)
        {
            var response = await _httpClient.PutAsync($"api/team/UpdateNumberOfChangesForAllTeams?numberOfChanges={numberOfChanges}", null);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return (true, content);
            }
            else
            {
                var result = JsonSerializer.Deserialize<JsonResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (false, result.Message);
            }
        }

        public async Task<(bool Success, string Message)> UpdateReportedToPlay(ReportedToPlayDto reportedToPlay)
        {
            var response = await _httpClient.PutAsJsonAsync("api/team/UpdateReportedToPlay", reportedToPlay);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return (true, content);
            }
            else
            {
                var result = JsonSerializer.Deserialize<JsonResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (false, result.Message);
            }
        }

        public async Task<(bool Success, string Message)> LeaveTeamByEmail(string email)
        {
            var response = await _httpClient.DeleteAsync($"api/team/LeaveTeamByEmail?email={email}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (result.Success, result.Message);
        }

        private class JsonResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
