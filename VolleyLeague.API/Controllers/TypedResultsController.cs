using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypedResultController : Controller
    {
        private readonly ILogger<TypedResultController> _logger;
        private readonly ITypedResultService _typedResultService;

        public TypedResultController(ILogger<TypedResultController> logger, ITypedResultService typedResultService)
        {
            _logger = logger;
            _typedResultService = typedResultService;
        }

        [AllowAnonymous]
        [HttpGet("GetTypedResults")]
        public async Task<IActionResult> GetTypedResults(int seasonId)
        {
            return Ok(await _typedResultService.GetTypedResults(seasonId));
        }

        [Authorize]
        [HttpPost("CreateTypedResult")]
        public async Task<IActionResult> CreateTypedResults([FromBody] TypedResultDto typedResult)
        {
            string? identity = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(identity))
            {
                return NotFound();
            }

            await _typedResultService.CreateTypedResult(typedResult, identity);
            return Ok();
        }

        [Authorize]
        [HttpGet("GetTypedResultByMatchAndUser")]
        public async Task<IActionResult> GetTypedResultByMatchAndUser(int matchId)
        {
            string? identity = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(identity))
            {
                return NotFound();
            }

            var result = await _typedResultService.GetTypedResultByMatchAndUserAsync(matchId, identity);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPut("UpdateTypedResult")]
        public async Task<IActionResult> UpdateTypedResult([FromBody] TypedResultDto typedResultDto)
        {
            string? identity = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(identity))
            {
                return NotFound();
            }

            var success = await _typedResultService.UpdateTypedResultAsync(typedResultDto, identity);
            if (!success)
            {
                return BadRequest("Failed to update the typed result.");
            }
            return Ok();
        }
    }
}
