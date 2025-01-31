﻿using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<List<SeasonDto>> GetAllSeasons();

        Task CreateSeason(SeasonDto season);

        Task UpdateSeason(SeasonDto season);

        Task DeleteSeason(int seasonId);
    }
}
