﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ILogger<SeasonService> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Season> _seasonRepository;
        public SeasonService(
            IMapper mapper,
            IBaseRepository<Season> seasonRepository
            )
        {
            _mapper = mapper;
            _seasonRepository = seasonRepository;
        }

        public async Task<List<SeasonDto>> GetAllSeasons()
        {
            var leagueList = await _seasonRepository.GetAll().ToListAsync();
            var leagueDtoList = _mapper.Map<List<SeasonDto>>(leagueList);
            return leagueDtoList;
        }

        public void CreateSeason(SeasonDto season)
        {
            var newSeason = _mapper.Map<Season>(season);
            Console.WriteLine(newSeason.Name);

            _seasonRepository.InsertAsync(newSeason);
        }

        public async Task UpdateSeason(SeasonDto season)
        {
            var seasonToUpdate = await _seasonRepository.GetById(season.Id);
            if (seasonToUpdate == null)
            {
                throw new KeyNotFoundException(ServicesConsts.League_not_found);
            }
            seasonToUpdate.Name = season.Name;
            await _seasonRepository.UpdateAsync(seasonToUpdate);
        }
    }
}