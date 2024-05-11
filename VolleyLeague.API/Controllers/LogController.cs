using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly ILogService _logService;

        public LogController(ILogger<LogController> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        [HttpGet("GetLastTenLogs")]
        public async Task<IActionResult> GetLastTenLogs()
        {
            var result = await _logService.GetLastTenLogs();
            return Ok(result);
        }

        [HttpPost("CreateLog")]
        public async Task<IActionResult> CreateLog()
        {
            var result = await _logService.GetLastTenLogs();
            return Ok(result);
        }
    }
}
