using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

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

        [AllowAnonymous]
        [HttpGet("GetAllSeasons")]
        public async Task<IActionResult> GetAllSeasons()
        {
            var result = await _seasonService.GetAllSeasons();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateSeason")]
        public IActionResult CreateSeason([FromBody] SeasonDto season)
        {
            _seasonService.CreateSeason(season);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason(SeasonDto updatedSeason)
        {
            await _seasonService.UpdateSeason(updatedSeason);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            try
            {
                await _seasonService.DeleteSeason(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
