using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Entities.Models;
using VolleyLeague.Services.Services;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDefinedCodeController : ControllerBase
    {
        private readonly IAdminDefinedCodeService _codeService;
        private readonly ILogger<AdminDefinedCodeController> _logger;

        public AdminDefinedCodeController(
            ILogger<AdminDefinedCodeController> logger,
            IAdminDefinedCodeService codeService)
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
        public async Task<IActionResult> UpdateCode([FromBody] AdminDefinedCode code)
        {
            if (code == null)
            {
                return BadRequest("Nieprawid³owe dane kodu.");
            }

            try
            {
                await _codeService.UpdateCodeAsync(code);
                return Ok("Kod zosta³ zaktualizowany pomyœlnie.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Wewnêtrzny b³¹d serwera");
            }
        }
    }
}