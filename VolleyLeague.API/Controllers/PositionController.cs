using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Services;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionController> _logger;
        public PositionController(
            ILogger<PositionController> logger,
            IPositionService positionService)
        {
            _logger = logger;
            _positionService = positionService;
        }

        [HttpGet("GetAllPositions")]
        public async Task<IActionResult> GetAllPositions() 
        { 
            var result = await _positionService.GetAllPositions();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPositionById")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var result = await _positionService.GetAllPositions();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosition(PositionDto position)
        {
            await _positionService.CreatePosition(position);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var result = await _positionService.DeletePosition(id);
            if(result)
            return Ok();    
            else return BadRequest();
        }
    }
}
