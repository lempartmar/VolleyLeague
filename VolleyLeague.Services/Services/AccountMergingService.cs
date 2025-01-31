﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class AccountMergingService : IAccountMergingService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<User> _usersRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        private readonly IBaseRepository<TeamPlayer> _teamPlayerRepository;
        private readonly ILogService _logService;

        public AccountMergingService(IBaseRepository<User> usersRepository, IBaseRepository<Credentials> credentialsRepository, IBaseRepository<TeamPlayer> teamPlayerRepository, ILogService logService, IMapper mapper)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _credentialsRepository = credentialsRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _logService = logService;
        }

        public async Task<TeamsToMergeDto> GetHasAccountsForMerging(string email)
        {
            TeamsToMergeDto dto = new TeamsToMergeDto();
            var usersWithSameEmail = await _usersRepository.GetAll()
                .Include(u => u.Credentials)
                .Include(u => u.TeamPlayers)
                .Where(u => u.AdditionalEmail == email)
                .ToListAsync();

            if (usersWithSameEmail.Count < 2)
            {
                dto.Status = false;
                return dto;
            }

            var hasCredentialsUser = usersWithSameEmail.Any(u => u.Credentials != null);
            var hasTeamPlayerUser = usersWithSameEmail.Any(u => u.TeamPlayers.Any());

            dto.Status = hasCredentialsUser && hasTeamPlayerUser;

            var usersByEmailFromTeam = await _teamPlayerRepository
                .GetAll()
                .Include(u => u.Team)
                .Include(u => u.Player)
                .Where(u => u.Player.AdditionalEmail == email)
                .FirstOrDefaultAsync();

            dto.TeamName = usersByEmailFromTeam.Team.Name;
            return dto;
        }

        public async Task<string> GetInfoAboutTheMergedTeam(string email)
        {
            var usersByEmailFromTeam = await _teamPlayerRepository
                .GetAll()
                .Include(u => u.Team)
                .Include(u => u.Player)
                .Where(u => u.Player.AdditionalEmail == email)
                .FirstOrDefaultAsync();

            return usersByEmailFromTeam.Team.Name;
        }

        public async Task<bool> AccountMerging(string email)
        {
            var usersWithSameEmail = await _usersRepository.GetAll()
                .Include(u => u.Credentials)
                .Include(u => u.TeamPlayers)
                .Where(u => u.AdditionalEmail == email)
                .ToListAsync();

            if (usersWithSameEmail.Count < 2)
            {
                return false;
            }

            var userWithCredentials = usersWithSameEmail.FirstOrDefault(u => u.Credentials != null);
            var userWithoutCredentials = usersWithSameEmail.FirstOrDefault(u => u.Credentials == null);

            var playerWithoutCredentials = await _usersRepository.GetAll()
                .Include(u => u.Credentials)
                .Include(u => u.TeamPlayers)
                .Where(u => u.AdditionalEmail == email)
                .Where(x => x.Credentials == null)
                .FirstOrDefaultAsync();

            if (userWithCredentials == null || userWithoutCredentials == null)
            {
                return false;
            }

            var playerId = userWithoutCredentials.Id;
            var teamPlayerUser = await _teamPlayerRepository.GetAll().Where(x => x.PlayerId == playerId).FirstOrDefaultAsync();

            playerWithoutCredentials.TeamPlayers.FirstOrDefault().PlayerId = userWithCredentials.Id;
          
            await _usersRepository.Delete(userWithoutCredentials);
            await _usersRepository.SaveChangesAsync();

            return true;
        }
    }
}
