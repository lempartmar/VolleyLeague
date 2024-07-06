using VolleyLeague.Shared.Dtos.Files;

namespace VolleyLeague.Services.Interfaces
{
    public interface IYoutubeService
    {
        Task<List<YouTubeVideoDto>> GetLatestVideosAsync(int maxResults);
    }
}
