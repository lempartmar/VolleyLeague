﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ILogger<MatchController> _logger;
        public MatchController(
            ILogger<MatchController> logger,
            IMatchService matchService)
        {
            _logger = logger;
            _matchService = matchService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var result = await _matchService.GetMatchByIdAsync(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetNextTwoMatches")]
        public async Task<IActionResult> GetNextTwoMatches()
        {
            var result = await _matchService.GetNextTwoMatchesAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetLastMatch")]
        public async Task<IActionResult> GetLastMatch()
        {
            var result = await _matchService.GetLastMatchAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllMatches")]
        public async Task<IActionResult> GetAllMatches()
        {
            var result = await _matchService.GetAllMatchesAsync();
            return Ok(result);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("createMatch")]
        public async Task<IActionResult> CreateMatch([FromBody] NewMatchDto match)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }

            await _matchService.AddMatch(match);
            return Ok();
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateMatch(ManageMatchDto match)
        //{
        //    string? id = User.Identity?.Name;
        //    if (string.IsNullOrWhiteSpace(id))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _matchService.UpdateMatch(match);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            string? userId = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }
            var result = await _matchService.DeleteMatch(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetTeamsInRound/{roundId}")]
        public async Task<IActionResult> GetTeamsInRound(int roundId)
        {
            var matches = await _matchService.GetMatchesByRoundId(roundId);
            var teamsInRound = matches
                .SelectMany(m => new[] { m.HomeTeamId, m.GuestTeamId })
                .Distinct()
                .ToList();
            return Ok(teamsInRound);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetReferees")]
        public async Task<IActionResult> GetReferees()
        {
            //string? userId = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(userId))
            //{
            //    return Unauthorized();
            //}
            var data = await _matchService.GetOtherData();
            return Ok(await _matchService.GetReferees());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveReferee/{userId}")]
        public async Task<IActionResult> DeleteReferee(int userId)
        {
            bool result = await _matchService.RemoveReferee(userId);
            if (result)
            {
                return Ok(new { success = true, message = "Rola sędziego została usunięta." });
            }
            else
            {
                return NotFound(new { success = false, message = "Użytkownik nie znaleziony lub nie posiada roli sędziego." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPotentialReferees")]
        public async Task<IActionResult> GetPotentialReferees()
        {
            return Ok(await _matchService.GetPotentialReferees());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AddReferee")]
        public async Task<IActionResult> AddReferee(int userId)
        {
            //string? userId = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(userId))
            //{
            //    return Unauthorized();
            //}

            return Ok(await _matchService.AddReferee(userId));
        }

        [AllowAnonymous]
        [HttpGet("matchesByCriteria")]
        public async Task<IActionResult> GetMatches([FromQuery] int leagueId, [FromQuery] int seasonId, [FromQuery] int roundId, [FromQuery] int teamId)
        {
            if (leagueId == 0 && seasonId == 0 && roundId == 0 && teamId == 0)
            {
                return Ok(await _matchService.GetAllMatchesAsync());
            }

            if (seasonId != 0 && teamId != 0)
            {
                return Ok(await _matchService.GetMatches(seasonId, teamId));
            }

            return Ok(await _matchService.GetMatches(leagueId, seasonId, roundId));
        }

        [AllowAnonymous]
        [HttpGet("matchesByCriteria2")]
        public async Task<IActionResult> GetMatches([FromQuery] int leagueId, [FromQuery] int seasonId, [FromQuery] int roundId)
        {
            if (leagueId == 0 && seasonId == 0 && roundId == 0)
            {
                return Ok(await _matchService.GetAllMatchesAsync());
            }

            return Ok(await _matchService.GetMatches(leagueId, seasonId, roundId));
        }

        [AllowAnonymous]
        [HttpGet("getStandings")]
        public async Task<IActionResult> GetStandings([FromQuery] int leagueId, [FromQuery] int seasonId)
        {
            if (leagueId == 0 && seasonId == 0)
            {
                return Ok();
            }

            return Ok(await _matchService.GetStandings(seasonId, leagueId));
        }

        [AllowAnonymous]
        [HttpGet("get10LastMatches")]
        public async Task<IActionResult> Get10LastMatches()
        {
            return Ok(await _matchService.GetLast10Matches());
        }

        [AllowAnonymous]
        [HttpGet("getMvpBySeasonAndLeague")]
        public async Task<IActionResult> GetMvpBySeasonAndLeague([FromQuery] int seasonId, [FromQuery] int leagueId)
        {
            var mvps = await _matchService.GetMvpBySeasonAndLeague(seasonId, leagueId);
            return Ok(mvps);
        }
    }
}
