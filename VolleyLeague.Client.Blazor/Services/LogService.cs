﻿using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface ILogService
    {
        public Task<List<LogDto>> GetLogs();
    }
    public class LogService : ILogService
    {
        private readonly HttpClient _httpClient;

        public LogService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<List<LogDto>> GetLogs()
        {
            var response = await _httpClient.GetAsync("api/log/GetLastTenLogs");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var logs = JsonSerializer.Deserialize<List<LogDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return logs;
        }
}
}
