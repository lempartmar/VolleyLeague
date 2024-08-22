using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [AllowAnonymous]
        [HttpGet("GetAllTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _teamService.GetAllTeams();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllTeamsForEdit")]
        public async Task<IActionResult> GetAllTeamsForEdit()
        {
            var result = await _teamService.GetAllExtendedTeams();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddTeam")]
        public async Task<IActionResult> AddTeam([FromBody] NewTeamDto team)
        {
            string? email = User.Identity?.Name;
            var result =  await _teamService.AddTeam(team, email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetTeamById/{Id}")]
        public async Task<IActionResult> GetTeamById(int Id)
        {
            var result = await _teamService.GetTeamById(Id);
            return Ok(result);
        }

        [AllowAnonymous]
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

            var result = await _teamService.AddTeam(team, email);

            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpPut("UpdateTeam")]
        public async Task<IActionResult> UpdateTeam([FromBody] ManageTeamDto team)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }
            var result = await _teamService.UpdateTeam(team, id);

            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpPut("UpdateExtendedTeam")]
        public async Task<IActionResult> UpdateExtendedTeam([FromBody] ExtendedTeamDto team)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }
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
            return BadRequest("Nie udało się usunąć zespołu.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getallteamsimagesstatus")]
        public async Task<IActionResult> GetAllTeamsImagesStatus()
        {
            var result = await _teamService.GetAllTeamsImagesStatus();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DownloadTeamImage/{teamId}")]
        public async Task<IActionResult> DownloadTeamImage(int teamId)
        {
            var teamImage = await _teamService.GetTeamImageByTeamId(teamId);

            if (teamImage == null || teamImage.Image == null)
            {
                return NotFound();
            }

            var memoryStream = new MemoryStream(teamImage.Image);
            return File(memoryStream, teamImage.ImageType, $"{teamId}.jpg");
        }

        [Authorize]
        [HttpPut("UpdateCaptain")]
        public async Task<IActionResult> UpdateCaptain([FromQuery] int newCaptainId)
        {
            string? email = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var result = await _teamService.UpdateCaptain(newCaptainId, email);

            if (result)
            {
                return Ok(new { Success = true, Message = "Captain updated successfully." });
            }
            return BadRequest(new { Success = false, Message = "Failed to update captain." });
        }


        [Authorize]
        [HttpPost("UploadTeamImage/{teamId}")]
        public async Task<IActionResult> UploadTeamImage(int teamId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _teamService.UploadTeamImage(teamId, file);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpGet("IsReportedToPlay/{teamId}")]
        public async Task<IActionResult> IsReportedToPlay(int teamId)
        {
            var result = await _teamService.IsReportedToPlay(teamId);

            if (result)
            {
                return Ok(new { Success = true, IsReportedToPlay = result });
            }
            return NotFound(new { Success = false, Message = "Team not found or not reported to play." });
        }

        [Authorize]
        [HttpPut("UpdateReportedToPlay")]
        public async Task<IActionResult> UpdateReportedToPlay([FromBody] ReportedToPlayDto reportedToPlay)
        {
            if (reportedToPlay == null || reportedToPlay.TeamId <= 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid data." });
            }

            var result = await _teamService.UpdateReportedToPlay(reportedToPlay);

            if (result)
            {
                return Ok(new { Success = true, Message = "ReportedToPlay status updated successfully." });
            }
            return BadRequest(new { Success = false, Message = "Failed to update ReportedToPlay status." });
        }

        [Authorize]
        [HttpDelete("DeleteTeamImage/{teamId}")]
        public async Task<IActionResult> DeleteTeamImage(int teamId)
        {
            var result = await _teamService.DeleteTeamImage(teamId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpDelete("LeaveTeamByEmail")]
        public async Task<IActionResult> LeaveTeamByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Success = false, Message = "Email is required." });
            }

            var result = await _teamService.LeaveTeamByEmail(email);
            if (result.Success)
            {
                return Ok(new { result.Success, result.Message });
            }
            return BadRequest(new { result.Success, result.Message });
        }
    }
}
