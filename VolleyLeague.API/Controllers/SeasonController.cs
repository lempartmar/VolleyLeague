using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Services.Services;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonService _seasonService;
        private readonly ILogger<SeasonController> _logger;
        public SeasonController(
            ILogger<SeasonController> logger,
            ISeasonService seasonService)
        {
            _logger = logger;
            _seasonService = seasonService;
        }

        [HttpGet("GetAllSeasons")]
        public async Task<IActionResult> GetAllSeasons()
        {
            var result = await _seasonService.GetAllSeasons();
            return Ok(result);
        }

        [HttpPost("CreateSeason")]
        public IActionResult CreateSeason([FromBody] SeasonDto season)
        {
            _seasonService.CreateSeason(season);
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason(SeasonDto updatedSeason)
        {
            await _seasonService.UpdateSeason(updatedSeason);
            return Ok();
        }
    }
}
