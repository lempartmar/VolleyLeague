using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Models;
using VolleyLeague.Services.Services;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDefinedCodeController : ControllerBase
    {
        private readonly IUserDefinedCodeService _codeService;
        private readonly ILogger<UserDefinedCodeController> _logger;

        public UserDefinedCodeController(
            ILogger<UserDefinedCodeController> logger,
            IUserDefinedCodeService codeService)
        {
            _logger = logger;
            _codeService = codeService;
        }

        [HttpGet("GetCodeByKey/{key}")]
        public async Task<IActionResult> GetCodeByKey(string key)
        {
            var code = await _codeService.GetCodeByKeyAsync(key);
            if (code == null)
            {
                return NotFound();
            }
            return Ok(code);
        }

        [HttpPut("UpdateCode")]
        public async Task<IActionResult> UpdateCode([FromBody] UserDefinedCode code)
        {
            if (code == null)
            {
                return BadRequest("Invalid code data.");
            }

            try
            {
                await _codeService.UpdateCodeAsync(code);
                return Ok("Code updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating code");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}