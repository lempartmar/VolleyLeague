using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;

namespace VolleyLeague.Services.Services
{
    public class RoundService : IRoundService
    {
        private readonly ILogger<RoundService> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Round> _roundRepository;
        private readonly IBaseRepository<Match> _matchRepository;

        public RoundService(
            IMapper mapper,
            IBaseRepository<Round> roundRepository,
            IBaseRepository<Match> matchRepository
            )
        {
            _mapper = mapper;
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;

        }

        public async Task<List<RoundDto>> GetAllRounds()
        {
            var roundAllList = await _roundRepository.GetAll().ToListAsync();
            var roundDtoList = _mapper.Map<List<RoundDto>>(roundAllList);
            return roundDtoList;
        }

        public async Task<List<RoundDto>> GetRoundsBySeasonId(int? seasonId)
        {
            if (seasonId == null)
            {
                var rounds = await _roundRepository.GetAllDescending().Where(r => r.Season.Id == seasonId).ToListAsync();
                var roundsAllList = rounds.Select(r => (RoundDto)r).ToList();
                var result = _mapper.Map<List<RoundDto>>(roundsAllList);
                return result;
            }
            var roundAllList = await _roundRepository.GetAll().Where(x => x.SeasonId == seasonId).ToListAsync();
            var roundDtoList = _mapper.Map<List<RoundDto>>(roundAllList);
            return roundDtoList;
        }

        public async Task CreateRound(RoundDto round)
        {
            var newRound = _mapper.Map<Round>(round);
            Console.WriteLine(newRound.Name);

            await _roundRepository.InsertAsync(newRound);
            await _roundRepository.SaveChangesAsync();
        }

        public async Task UpdateRound(RoundDto round)
        {
            using (var context = new VolleyballContext())
            {
                var roundToUpdate = await context.Rounds.FindAsync(round.Id);

                if (roundToUpdate == null)
                {
                    throw new KeyNotFoundException(ServicesConsts.League_not_found);
                }

                roundToUpdate.Name = round.Name;

                await context.SaveChangesAsync();
            }
        }

        public async Task<string> DeletePosition(int id)
        {
            var round = await _roundRepository.GetById(id);

            if (round == null)
            {
                return "Runda nie znaleziona";
            }

            var matches = await _matchRepository.GetAll().Where(m => m.RoundId == id).ToListAsync();
            if (matches.Any())
            {
                return "Nie można usunąć rundy, ponieważ ma przypisane mecze";
            }

            try
            {
                await _roundRepository.Delete(round);
                await _roundRepository.SaveChangesAsync();
                return "Runda została pomyślnie usunięta";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas usuwania rundy o id {id}");
                return "Wystąpił błąd podczas usuwania rundy";
            }
        }
    }
}