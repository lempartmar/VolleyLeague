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

        [HttpGet("GetTypedResults")]
        public async Task<IActionResult> GetTypedResults(int seasonId)
        {
            return Ok(await _typedResultService.GetTypedResults(seasonId));
        }

        [HttpPost("CreateTypedResult")]
        public async Task<IActionResult> CreateTypedResults([FromBody] TypedResultDto typedResult)
        {
            await _typedResultService.CreateTypedResult(typedResult);
            return Ok();
        }
    }
}
