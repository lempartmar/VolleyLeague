using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Services.Interfaces;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var result = await _matchService.GetMatchByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllMatches")]
        public async Task<IActionResult> GetAllMatches()
        {
            var result = await _matchService.GetAllMatchesAsync();
            return Ok(result);
        }

        [HttpPost("createMatch")]
        public async Task<IActionResult> CreateMatch([FromBody] NewMatchDto match)
        {
            //string? id = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return Unauthorized();
            //}
            
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

        [HttpDelete]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            string? userId = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }
            var result = await _matchService.DeleteMatch(id);
            return  Ok(result);
        }

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

        [HttpGet("GetPotentialReferees")]
        public async Task<IActionResult> GetPotentialReferees()
        {
            return Ok(await _matchService.GetPotentialReferees());
        }

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

        [HttpGet("matchesByCriteria2")]
        public async Task<IActionResult> GetMatches([FromQuery] int leagueId, [FromQuery] int seasonId, [FromQuery] int roundId)
        {
            if (leagueId == 0 && seasonId == 0 && roundId == 0)
            {
                return Ok(await _matchService.GetAllMatchesAsync());
            }

            return Ok(await _matchService.GetMatches(leagueId, seasonId, roundId));
        }

        [HttpGet("getStandings")]
        public async Task<IActionResult> GetStandings([FromQuery] int leagueId, [FromQuery] int seasonId)
        {
            if (leagueId == 0 && seasonId == 0)
            {
                return Ok();
            }

            return Ok(await _matchService.GetStandings(seasonId, leagueId));
        }

        [HttpGet("get10LastMatches")]
        public async Task<IActionResult> Get10LastMatches()
        {
            return Ok(await _matchService.GetLast10Matches());
        }
    }
}
