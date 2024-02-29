using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;
using System.Text.Json;

namespace VolleyballBlazor.Infrastructure.Client.Services
{
    public interface IMatchService
    {
        public Task<List<VenueDto>> GetVenues();
        public Task<List<RoundDto>> GetRounds();
        public Task<List<RoundDto>> GetRounds(int seasonId);
        public Task<List<LeagueDto>> GetLeagues();
        public Task<List<SeasonDto>> GetSeasons();
        public Task<MatchDto> GetMatch(int matchId);
        public Task<List<MatchSummaryDto>> GetMatches(int seasonId, int leagueId, int roundId);
        public Task<List<MatchSummaryDto>> GetMatches(int seasonId, int teamId);
        public Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId);
    }
    public class MatchService : IMatchService
    {
        private readonly HttpClient httpClient;

        public MatchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<VenueDto>> GetVenues()
        {
            var response = await httpClient.GetAsync("api/venue");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var venues = JsonSerializer.Deserialize<List<VenueDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return venues;
        }

        public async Task<List<RoundDto>> GetRounds()
        {
            var response = await httpClient.GetAsync("api/round");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rounds = JsonSerializer.Deserialize<List<RoundDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return rounds;
        }

        public async Task<List<RoundDto>> GetRounds(int seasonId)
        {
            var response = await httpClient.GetAsync($"api/round?seasonId={seasonId}");
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

        public async Task<MatchDto> GetMatch(int matchId)
        {
            var response = await httpClient.GetAsync($"api/match/{matchId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var match = JsonSerializer.Deserialize<MatchDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return match;
        }

        public async Task<List<MatchSummaryDto>> GetMatches(int seasonId, int leagueId, int roundId)
        {
            var response = await httpClient.GetAsync($"api/match?seasonId={seasonId}&leagueId={leagueId}&roundId={roundId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var matches = JsonSerializer.Deserialize<List<MatchSummaryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return matches;
        }

        public async Task<List<MatchSummaryDto>> GetMatches(int seasonId, int teamId)
        {
            var response = await httpClient.GetAsync($"api/match?seasonId={seasonId}&teamId={teamId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var matches = JsonSerializer.Deserialize<List<MatchSummaryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return matches;
        }

        public async Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId)
        {
            var response = await httpClient.GetAsync($"api/Match/getStandings?leagueId={leagueId}&seasonId={seasonId}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var standings = JsonSerializer.Deserialize<List<StandingsDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return standings;
        }
    }
}
