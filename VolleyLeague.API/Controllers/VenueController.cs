using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        public IVenueService _venueService;
        public ILogger<VenueController> Logger;

        public VenueController(IVenueService venueService, ILogger<VenueController> logger)
        {
            _venueService = venueService;
            Logger = logger;

        }

        [HttpGet("GetAllVenues")]
        public async Task<IActionResult> GetAllVenues()
        {
            var result = await _venueService.GetSportsVenueAsync();
            return Ok(result);
        }
    }
}
