using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoundController : ControllerBase
    {
        private readonly IRoundService _roundService;
        private readonly ILogger<RoundController> _logger;
        public RoundController(
            ILogger<RoundController> logger,
            IRoundService roundService)
        {
            _logger = logger;
            _roundService = roundService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllRounds")]
        public async Task<IActionResult> GetAllRounds()
        {
            var result = await _roundService.GetAllRounds();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetRoundsBySeasonId/{seasonId}")]
        public async Task<IActionResult> GetRoundsBySeasonId(int? seasonId)
        {
            var result = await _roundService.GetRoundsBySeasonId(seasonId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateRound")]
        public IActionResult CreateSeason([FromBody] RoundDto round)
        {
            _roundService.CreateRound(round);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRound(RoundDto roundDto)
        {
            await _roundService.UpdateRound(roundDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRound(int id)
        {
            var result = await _roundService.DeletePosition(id);
            if (result == "Runda została pomyślnie usunięta")
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
