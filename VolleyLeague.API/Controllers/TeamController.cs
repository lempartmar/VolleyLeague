using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamController> _logger;
        public TeamController(
            ILogger<TeamController> logger,
            ITeamService teamService)
        {
            _logger = logger;
            _teamService = teamService;
        }

        [HttpGet("GetAllTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _teamService.GetAllTeams();
            return Ok(result);
        }

        [HttpGet("GetAllTeamsForEdit")]
        public async Task<IActionResult> GetAllTeamsForEdit()
        {
            var result = await _teamService.GetAllExtendedTeams();
            return Ok(result);
        }

        [HttpPost("AddTeam")]
        public async Task<IActionResult> AddTeam([FromBody] NewTeamDto team) 
        {
            await _teamService.AddTeam(team);
            return Ok();
        }

        [HttpGet("GetTeamById/{Id}")]
        public async Task<IActionResult> GetTeamById(int Id)
        {
            var result = await _teamService.GetTeamById(Id);
            return Ok(result);
        }

        [HttpGet("GetTeamsByLeagueId/{Id}")]
        public async Task<IActionResult> GetTeamsByLeagueId(int Id)
        {
            var result = await _teamService.GetTeamsByLeagueId(Id);
            return Ok(result);
        }

        [HttpPost("CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] NewTeamDto team)
        {
            string? id = "nowa@mail.com";

            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }

            await _teamService.AddTeam(team);

            return Ok();
        }

        [HttpPut("UpdateTeam")]
        public async Task<IActionResult> UpdateTeam([FromBody] ManageTeamDto team)
        {
            string? id = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return Unauthorized();
            //}
            var result = await _teamService.UpdateTeam(team);

            return Ok();
        }

        [HttpPut("UpdateExtendedTeam")]
        public async Task<IActionResult> UpdateExtendedTeam([FromBody] ExtendedTeamDto team)
        {
            string? id = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return Unauthorized();
            //}
            var result = await _teamService.UpdateExtendedTeam(team);

            return Ok();
        }

        [HttpPut]
        [Route("UpdateTeamPlayer")]
        public async Task<IActionResult> UpdateTeamPlayer([FromBody] PlayerSummaryDto teamPlayer)
        {
            var result = await _teamService.UpdateTeamPlayer(teamPlayer);

            return Ok();
        }
    }
}
