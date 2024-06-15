using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.Services.Services
{
    public class VenueService : IVenueService
    {
        private readonly IBaseRepository<SportsVenue> _sportsVenueRepository;
        private readonly ILogger<VenueService> _logger;
        private readonly IMapper _mapper;
        
        public VenueService(IBaseRepository<SportsVenue> sportsVenueRepository,  ILogger<VenueService> logger, IMapper mapper)
        {
            _sportsVenueRepository = sportsVenueRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<VenueDto>> GetSportsVenueAsync()
        {
            var result = await _sportsVenueRepository.GetAll().ToListAsync();
            var venueDto = _mapper.Map<List<VenueDto>>(result);

            return venueDto;
        }
    }
}
