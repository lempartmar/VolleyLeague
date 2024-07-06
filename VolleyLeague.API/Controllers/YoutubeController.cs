using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Services;
using VolleyLeague.Shared.Dtos.Files;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YouTubeController : ControllerBase
    {
        private readonly YoutubeService _youTubeService;

        public YouTubeController(YoutubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

        [HttpGet("latest-videos")]
        public async Task<ActionResult<List<YouTubeVideoDto>>> GetLatestVideos()
        {
            var videos = await _youTubeService.GetLatestVideosAsync();
            return Ok(videos);
        }
    }
}