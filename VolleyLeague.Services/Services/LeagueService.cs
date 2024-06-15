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
    public class LeagueService : ILeagueService
    {
        private readonly ILogger<LeagueService> _logger;    
        private readonly IMapper _mapper;
        private readonly IBaseRepository<League> _leagueRepository;
        public LeagueService(
            IMapper mapper,
            IBaseRepository<League> leagueRepository
            ) 
        { 
            _mapper = mapper;
            _leagueRepository = leagueRepository;
        }

        public async Task<List<LeagueDto>> GetAllLeagues()
        {
            var leagueList = await _leagueRepository.GetAll().ToListAsync();
            var leagueDtoList = _mapper.Map<List<LeagueDto>>(leagueList);
            return leagueDtoList;
        }

        public async Task CreateLeague(LeagueDto league)
        {
            var newLeague = _mapper.Map<League>(league);
            Console.WriteLine(newLeague.Name);
            
            await _leagueRepository.InsertAsync(newLeague);
            await _leagueRepository.SaveChangesAsync();
        }

        public async Task UpdateLeague(LeagueDto league)
        {
            var leagueToUpdate = await _leagueRepository.GetById(league.Id);
            if (leagueToUpdate == null)
            {
                throw new KeyNotFoundException(ServicesConsts.League_not_found);
            }
            leagueToUpdate.Name = league.Name;
            await _leagueRepository.UpdateAsync(leagueToUpdate);   
        }

        public async Task<bool> DeleteLeague(int id)
        {
            var result = true;
            var league = await _leagueRepository.GetById(id);

            if (league != null)
            {
                try
                {
                    await _leagueRepository.Delete(league);
                }
                catch (Exception ex)
                {
                    result = false;
                    _logger.LogError(ex.Message, "Error when deleting league");
                }

                return result;
            }

            return false;
        }
    }
}
