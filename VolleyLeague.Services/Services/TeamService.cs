using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Team> _teamRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        public TeamService(IMapper mapper, IBaseRepository<Team> teamRepository, IBaseRepository<User> userRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TeamDto>> GetAllTeams()
        {
            var allTeams = await _teamRepository.GetAll().ToListAsync();
            return _mapper.Map<List<TeamDto>>(allTeams);
        }

        public async Task<TeamDto> GetTeamById(int Id)
        {
            var team = await _teamRepository.GetAll()
                .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
                .Include(t => t.Captain).ThenInclude(c => c.Position)
                .Include(t => t.League)
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (team == null)
            {
                return null;
            }

            return _mapper.Map<TeamDto>(team);
        }

        public async Task AddTeam(NewTeamDto team)
        {
            var teamPlayers = new List<TeamPlayer>();
            var newUsersToSendInvitation = new List<TeamPlayerDto>();

            foreach (var player in team.Players)
            {
                if (player.Id != null)
                {
                    var user = await _userRepository.GetAll().Where(u => u.Id == player.Id).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        var teamPlayer = _mapper.Map<TeamPlayer>(user);
                        teamPlayers.Add(teamPlayer);
                        continue;
                    }
                }

                else
                {
                    //newUsersToSendInvitation.Add(player);
                    TeamPlayer teamPlayer = _mapper.Map<TeamPlayer>(player);
                    teamPlayers.Add(teamPlayer);
                }
            }

            var newTeam = new Team();
            newTeam.TeamPlayers = teamPlayers;
            var captainUser = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(u => u.Email == "nowa@mail.com");
            newTeam.Captain = captainUser.User;

            var newTeamToDb = _mapper.Map(team, newTeam);

            try
            {
                await _teamRepository.InsertAsync(newTeamToDb);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        


        public async Task<bool> DeleteTeam(int teamId)
        {
            var teamToDelete = await _teamRepository.GetAll().FirstOrDefaultAsync(t => t.Id == teamId);

            if (teamToDelete == null)
            {
                return false;
            }

            try
            {
                await _teamRepository.Delete(teamToDelete);
                await _teamRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTeam(ManageTeamDto teamDto)
        {
            var teamToUpdate = await _teamRepository.GetAll()
                .Include(u => u.Captain).ThenInclude(c => c.Credentials)
                .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials.Email == "nowy@mail.com");

            if (teamToUpdate == null)
            {
                return false;
            }

            _mapper.Map(teamDto, teamToUpdate);

            var existingPlayerIds = teamToUpdate.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            foreach (var playerDto in teamDto.Players.Where(p => existingPlayerIds.Contains((int)p.Id)))
            {
                var player = teamToUpdate.TeamPlayers.FirstOrDefault(tp => tp.PlayerId == playerDto.Id)?.Player;
                if (player != null)
                {
                    _mapper.Map(playerDto, player);
                }
            }

            foreach (var newPlayerDto in teamDto.NewPlayers)
            {
                if (!existingPlayerIds.Contains((int)newPlayerDto.Id))
                {
                    var newPlayer = _mapper.Map<User>(newPlayerDto);
                    teamToUpdate.TeamPlayers.Add(new TeamPlayer { Player = newPlayer, JoinDate = DateTime.UtcNow });
                }
            }

            foreach (var player in teamDto.RemovedPlayers)
            {
                var playerToRemove = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Player.Id == player.Id);
                if (playerToRemove != null)
                {
                    teamToUpdate.TeamPlayers.Remove(playerToRemove);
                }
            }

            try
            {
                await _teamRepository.UpdateAsync(teamToUpdate);
                await _teamRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public async Task<bool> UpdateTeamPlayer(PlayerSummaryDto userSummary)
        {

            var player = _userRepository.GetAll().FirstOrDefault(u => u.Id == userSummary.Id);

            if (player == null)
            {
                return false;
            }

            _mapper.Map(userSummary, player);

            if (!string.IsNullOrEmpty(userSummary.PositionName))
            {
                player.Position = await _positionRepository.GetAll().FirstOrDefaultAsync(p => p.Name == userSummary.PositionName);
            }

            try
            {
                await _userRepository.SaveChangesAsync();
                await _positionRepository.SaveChangesAsync();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ManagedTeamDataDto> GetTeamByCaptain(string email)
        {
            var team = await _teamRepository.GetAll()
                .Include(t => t.League)
                .Include(u => u.Captain).ThenInclude(c => c.Credentials)
                .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            var result = _mapper.Map<ManagedTeamDataDto>(team);
            return result;
        }

        public async Task<List<TeamDto>> GetTeamsByLeagueId(int leagueId)
        {
            var teams = await _teamRepository.GetAll()
                .Include(t => t.League)
                .Include(t => t.TeamPlayers).ThenInclude(p => p.Player).ThenInclude(p => p.Position)
                .Include(t => t.Captain)
                .Where(t => t.League != null && t.League.Id == leagueId).ToListAsync();

            var result = _mapper.Map<List<TeamDto>>(teams);
            return result;
        }

        public async Task<bool> UpdateCaptain(int newCaptainId, string email)
        {
            var team = await _teamRepository.GetAll()
                .Include(t => t.League)
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (team == null)
            {
                return false;
            }

            var newCaptain = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Id == newCaptainId);

            var oldCaptainId = team.Captain.Id;

            if (newCaptain == null)
            {
                return false;
            }
            team.Captain = newCaptain;
            team.TeamPlayers.Remove(team.TeamPlayers.FirstOrDefault(p => p.PlayerId == newCaptainId)!);


            team.TeamPlayers.Add(new TeamPlayer
            {
                PlayerId = oldCaptainId,
                JoinDate = DateTime.Now
            });

            try
            {
                await _teamRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
