using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VolleyLeague.Services.Interfaces;
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
                return BadRequest("Email parameter is required.");
            }

            var result = await _accountMergingService.GetHasAccountsForMerging(email);
            return Ok(result);
        }

        [HttpGet("GetInfoAboutTeamsToMerge")]
        public async Task<IActionResult> GetInfoAboutTeamsToMerge([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email parameter is required.");
            }

            var result = await _accountMergingService.GetInfoAboutTheMergedTeam(email);
            return Ok(result);
        }

        [HttpDelete("AccountMerging")]
        public async Task<IActionResult> AccountMerging([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email parameter is required.");
            }

            var result = await _accountMergingService.AccountMerging(email);
            if (result)
            {
                return Ok("Accounts merged and unlinked account deleted successfully.");
            }
            return NotFound("Either accounts not found or merge criteria not met.");
        }
    }
}
