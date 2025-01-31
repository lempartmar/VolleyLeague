﻿using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.Services.Interfaces
{
    public interface IVenueService
    {
        Task<List<VenueDto>> GetSportsVenueAsync();
    }
}
