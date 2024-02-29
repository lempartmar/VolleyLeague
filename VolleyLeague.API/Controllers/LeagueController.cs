using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ILogger<LeagueController> _logger;
        public LeagueController(
            ILogger<LeagueController> logger,
            ILeagueService leagueService)
        {
            _logger = logger;
            _leagueService = leagueService;
        }

        [HttpGet("GetAllLeagues")]
        public async Task<IActionResult> GetAllLeagues() 
        { 
            var result = await _leagueService.GetAllLeagues();
            return Ok(result);
        }

        [HttpPost("CreateLeague")]
        public async Task<IActionResult> CreateLeague([FromBody] LeagueDto league)
        {
            await _leagueService.CreateLeague(league);
            return Ok();
        }

        [HttpDelete("DeleteLeague")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            var result = await _leagueService.DeleteLeague(id);
            if (result)
                return Ok();
            else return BadRequest();
        }

        [HttpPut("UpdateLeague")]
        public async Task<IActionResult> UpdateLeague([FromBody] LeagueDto league)
        {
            //string? id = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return Unauthorized();
            //}
            await _leagueService.UpdateLeague(league);

            return Ok();
        }
    }
}
