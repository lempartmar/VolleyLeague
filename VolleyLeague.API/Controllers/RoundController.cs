﻿using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Services.Interfaces;

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

        [HttpGet("GetAllRounds")]
        public async Task<IActionResult> GetAllRounds()
        {
            var result = await _roundService.GetAllRounds();
            return Ok(result);
        }

        [HttpGet("GetRoundsBySeasonId/{seasonId}")]
        public async Task<IActionResult> GetRoundsBySeasonId(int? seasonId)
        {
            var result = await _roundService.GetRoundsBySeasonId(seasonId);
            return Ok(result);
        }

        [HttpPost("CreateRound")]
        public IActionResult CreateSeason([FromBody] RoundDto round)
        {
            _roundService.CreateRound(round);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRound(int id)
        {
            var result = await _roundService.DeletePosition(id);
            if (result)
                return Ok();
            else return BadRequest();
        }
    }
}
