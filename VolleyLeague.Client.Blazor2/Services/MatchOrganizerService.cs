using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;
using System.Collections.Generic;

namespace VolleyLeague.Client.Blazor2.Services
{
    public interface IMatchOrganizerService
    {
        Task<List<VenueDto>> GetVenues();
        Task<List<RoundDto>> GetRounds();
        Task<List<LeagueDto>> GetLeagues();
        Task<List<PlayerSummaryDto>> GetReferees();
        Task<List<SeasonDto>> GetSeasons();
        Task<MatchDto> GetMatch(int matchId);
        Task<bool> CreateMatch(NewMatchDto match);
        Task<bool> UpdateMatch(ManageMatchDto match);
    }

    public class MatchOrganizerService : IMatchOrganizerService
    {
        private readonly HttpClient httpClient;

        public MatchOrganizerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<VenueDto>> GetVenues()
        {
            return await httpClient.GetFromJsonAsync<List<VenueDto>>("api/venue");
        }

        public async Task<List<RoundDto>> GetRounds()
        {
            return await httpClient.GetFromJsonAsync<List<RoundDto>>("api/round");
        }

        public async Task<List<LeagueDto>> GetLeagues()
        {
            return await httpClient.GetFromJsonAsync<List<LeagueDto>>("api/league");
        }

        public async Task<List<PlayerSummaryDto>> GetReferees()
        {
            return await httpClient.GetFromJsonAsync<List<PlayerSummaryDto>>("api/match/referees");
        }

        public async Task<List<SeasonDto>> GetSeasons()
        {
            return await httpClient.GetFromJsonAsync<List<SeasonDto>>("api/season/GetAllSeasons");
        }

        public async Task<MatchDto> GetMatch(int matchId)
        {
            return await httpClient.GetFromJsonAsync<MatchDto>($"api/match/{matchId}");
        }

        public async Task<bool> CreateMatch(NewMatchDto match)
        {
            var response = await httpClient.PostAsJsonAsync("api/match", match);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateMatch(ManageMatchDto match)
        {
            var response = await httpClient.PutAsJsonAsync("api/match", match);
            return response.IsSuccessStatusCode;
        }
    }
}
