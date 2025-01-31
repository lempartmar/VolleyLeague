﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

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

        [AllowAnonymous]
        [HttpGet("GetAllLeagues")]
        public async Task<IActionResult> GetAllLeagues()
        {
            var result = await _leagueService.GetAllLeagues();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateLeague")]
        public async Task<IActionResult> CreateLeague([FromBody] LeagueDto league)
        {
            string? idUser = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(idUser))
            {
                return Unauthorized();
            }

            await _leagueService.CreateLeague(league);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteLeague")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            string? idUser = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(idUser))
            {
                return Unauthorized();
            }

            var result = await _leagueService.DeleteLeague(id);
            if (result)
                return Ok();
            else return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateLeague")]
        public async Task<IActionResult> UpdateLeague([FromBody] LeagueDto league)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }
            await _leagueService.UpdateLeague(league);

            return Ok();
        }
    }
}
