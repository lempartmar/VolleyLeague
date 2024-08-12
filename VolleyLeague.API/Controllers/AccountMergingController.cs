using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Services;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountMergingController : ControllerBase
    {
        private readonly ILogger<AccountMergingController> _logger;
        private readonly IAccountMergingService _accountMergingService;

        public AccountMergingController(ILogger<AccountMergingController> logger, IAccountMergingService accountMergingService)
        {
            _logger = logger;
            _accountMergingService = accountMergingService;
        }

        [HttpGet("GetHasAccountsForMerging")]
        public async Task<IActionResult> GetHasAccountsForMerging([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Pole email jest wymagane.");
            }

            var result = await _accountMergingService.GetHasAccountsForMerging(email);
            return Ok(result);
        }

        [HttpGet("GetInfoAboutTeamsToMerge")]
        public async Task<IActionResult> GetInfoAboutTeamsToMerge([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Pole email jest wymagane.");
            }

            var result = await _accountMergingService.GetInfoAboutTheMergedTeam(email);
            return Ok(result);
        }

        [HttpDelete("AccountMerging")]
        public async Task<IActionResult> AccountMerging([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Pole email jest wymagane.");
            }

            var result = await _accountMergingService.AccountMerging(email);
            if (result)
            {
                return Ok("Konta zosta³y scalone, a niepowi¹zane konto zosta³o usuniête pomyœlnie.");
            }
            return NotFound("Nie znaleziono kont lub nie spe³niono kryteriów scalania.");
        }
    }
}
