﻿using System.Text.Json;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Files;

namespace VolleyLeague.Services.Services
{
    public class YoutubeService : IYoutubeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _channelId;

        public YoutubeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "";
            _channelId = "";
        }

        public async Task<List<YouTubeVideoDto>> GetLatestVideosAsync()
        {
            int maxResults = 5;
            var url = $"https://www.googleapis.com/youtube/v3/search?key={_apiKey}&channelId={_channelId}&part=snippet,id&order=date&maxResults={maxResults}";

            var response = await _httpClient.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);
            var items = jsonDoc.RootElement.GetProperty("items");

            var videos = new List<YouTubeVideoDto>();

            foreach (var item in items.EnumerateArray())
            {
                var video = new YouTubeVideoDto
                {
                    VideoId = item.GetProperty("id").GetProperty("videoId").GetString(),
                    Title = item.GetProperty("snippet").GetProperty("title").GetString(),
                    ThumbnailUrl = item.GetProperty("snippet").GetProperty("thumbnails").GetProperty("high").GetProperty("url").GetString()
                };
                videos.Add(video);
            }

            return videos;
        }
    }
}
