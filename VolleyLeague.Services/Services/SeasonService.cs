using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ILogger<SeasonService> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Season> _seasonRepository;
        private readonly IBaseRepository<Round> _roundRepository;

        public SeasonService(
            IMapper mapper,
            IBaseRepository<Season> seasonRepository,
            IBaseRepository<Round> roundRepository,
            ILogger<SeasonService> logger
        )
        {
            _mapper = mapper;
            _seasonRepository = seasonRepository;
            _roundRepository = roundRepository;
            _logger = logger;
        }

        public async Task<List<SeasonDto>> GetAllSeasons()
        {
            var leagueList = await _seasonRepository.GetAll().ToListAsync();
            var leagueDtoList = _mapper.Map<List<SeasonDto>>(leagueList);
            return leagueDtoList;
        }

        public async Task CreateSeason(SeasonDto season)
        {
            var newSeason = _mapper.Map<Season>(season);
            Console.WriteLine(newSeason.Name);

            await _seasonRepository.InsertAsync(newSeason);
            await _seasonRepository.SaveChangesAsync();
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
            await _seasonRepository.SaveChangesAsync();
        }

        public async Task DeleteSeason(int seasonId)
        {
            var seasonToDelete = await _seasonRepository.GetById(seasonId);
            if (seasonToDelete == null)
            {
                throw new KeyNotFoundException(ServicesConsts.League_not_found);
            }

            var rounds = await _roundRepository.GetAll().Where(r => r.SeasonId == seasonId).ToListAsync();
            if (rounds.Any())
            {
                throw new InvalidOperationException("Nie można usunąć sezonu, ponieważ posiada powiązane kolejki!");
            }

            try
            {
                await _seasonRepository.Delete(seasonToDelete);
                await _seasonRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, $"Error occurred while deleting season with id {seasonId}");
                throw new Exception("Błąd w trakcie usuwania sezonu!");
            }

        }
    }
}
