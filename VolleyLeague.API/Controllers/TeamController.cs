using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

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


        [Authorize]
        [HttpPost("AddTeam")]
        public async Task<IActionResult> AddTeam([FromBody] NewTeamDto team)
        {
            string? email = User.Identity?.Name;
            await _teamService.AddTeam(team, email);
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


        [Authorize]
        [HttpGet("GetManagedTeam")]
        public async Task<IActionResult> GetManagedTeam()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }

            var result = await _teamService.GetTeamByCaptain(id);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] NewTeamDto team)
        {
            string? email = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            await _teamService.AddTeam(team, email);

            return Ok();
        }

        [HttpPut("UpdateTeam")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTeam([FromBody] ManageTeamDto team)
        {
            string? id = User.Identity?.Name;
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return Unauthorized();
            //}
            var result = await _teamService.UpdateTeam(team, id);

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

        [Authorize]
        [HttpDelete("DeleteTeam/{Id}")]
        public async Task<IActionResult> DeleteTeam(int Id)
        {
            string? email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var result = await _teamService.DeleteTeam(Id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Failed to delete the team.");
        }

        [HttpDelete("LeaveTeamByEmail")]
        public async Task<IActionResult> LeaveTeam([FromQuery] string email)
        {
            // string? email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized("Email is required.");
            }

            var result = await _teamService.LeaveTeamByEmail(email);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}
