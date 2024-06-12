using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;
using System.Text.Json;
using System.Text;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface ISeasonService
    {
        public Task<List<RoundDto>> GetRounds();
        public Task<List<RoundDto>> GetRounds(int seasonId);
        public Task<List<LeagueDto>> GetLeagues();
        public Task<List<SeasonDto>> GetSeasons();
        public Task CreateSeason(SeasonDto season);
        public Task UpdateSeason(SeasonDto season);
        public Task<HttpResponseMessage> DeleteSeason(int seasonId);
        public Task CreateRound(RoundDto round); // Dodano metodę
        public Task UpdateRound(RoundDto round); // Dodano metodę
        Task<HttpResponseMessage> DeleteRound(int roundId);
    }

    public class SeasonService : ISeasonService
    {
        private readonly HttpClient httpClient;

        public SeasonService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RoundDto>> GetRounds()
        {
            var response = await httpClient.GetAsync("api/round/GetAllRounds");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rounds = JsonSerializer.Deserialize<List<RoundDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return rounds;
        }

        public async Task<List<RoundDto>> GetRounds(int seasonId)
        {
            var response = await httpClient.GetAsync($"api/round/GetRoundsBySeasonId/{seasonId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rounds = JsonSerializer.Deserialize<List<RoundDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return rounds;
        }

        public async Task<List<LeagueDto>> GetLeagues()
        {
            var response = await httpClient.GetAsync("api/League/GetAllLeagues");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var leagues = JsonSerializer.Deserialize<List<LeagueDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return leagues;
        }

        public async Task<List<SeasonDto>> GetSeasons()
        {
            var response = await httpClient.GetAsync("api/Season/GetAllSeasons");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var seasons = JsonSerializer.Deserialize<List<SeasonDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return seasons;
        }

        public async Task CreateSeason(SeasonDto season)
        {
            var seasonJson = JsonSerializer.Serialize(season);
            var content = new StringContent(seasonJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/Season/CreateSeason", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSeason(SeasonDto season)
        {
            var seasonJson = JsonSerializer.Serialize(season);
            var content = new StringContent(seasonJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/Season/{season.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> DeleteSeason(int seasonId)
        {
            var response = await httpClient.DeleteAsync($"api/Season/{seasonId}");
            return response;
        }

        public async Task CreateRound(RoundDto round)
        {
            var roundJson = JsonSerializer.Serialize(round);
            var content = new StringContent(roundJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/round/CreateRound", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateRound(RoundDto round)
        {
            var roundJson = JsonSerializer.Serialize(round);
            var content = new StringContent(roundJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/round/{round.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> DeleteRound(int roundId)
        {
            var response = await httpClient.DeleteAsync($"api/round/{roundId}");
            return response;
        }
    }
}
