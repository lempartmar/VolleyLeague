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
        private readonly IBaseRepository<League> _leagueRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        public TeamService(IMapper mapper, IBaseRepository<Team> teamRepository, IBaseRepository<League> leagueRepository, IBaseRepository<User> userRepository, IBaseRepository<Credentials> credentialsRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _leagueRepository = leagueRepository;
            _credentialsRepository = credentialsRepository;
        }

        public async Task<List<TeamDto>> GetAllTeams()
        {
            var allTeams = await _teamRepository.GetAll().ToListAsync();
            return _mapper.Map<List<TeamDto>>(allTeams);
        }

        public async Task<ExtendedTeamWithLeagueDto> GetAllExtendedTeams()
        {
            var allTeams = await _teamRepository.GetAll().Include(d => d.League).ToListAsync();
            var allLeagues = await _leagueRepository.GetAll().ToListAsync();
            var extendedTeams = _mapper.Map<List<ExtendedTeamDto>>(allTeams);
            var allLeaguesDtoList = _mapper.Map<List<LeagueDto>>(allLeagues);

            ExtendedTeamWithLeagueDto extendedTeam = new ExtendedTeamWithLeagueDto();
            extendedTeam.ExtendedTeamListDto = extendedTeams;
            extendedTeam.leagueDtos = allLeaguesDtoList; 
            
            return extendedTeam;
        }

        public async Task<TeamDto> GetTeamById(int Id)
        {
            Team team2 = new Team();
            var team = await _teamRepository.GetAll()
                .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player).ThenInclude(c => c.Position)
                .Include(t => t.Captain)
                .Include(t => t.League)
                .FirstOrDefaultAsync(t => t.Id == Id);

            //var team = await _teamRepository.GetAll()
            //    .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
            //    .Include(t => t.Captain).ThenInclude(c => c.Position)
            //    .Include(t => t.League)
            //    .FirstOrDefaultAsync(t => t.Id == Id);

            if (team == null)
            {
                return null;
            }

            TeamDto teamDto = _mapper.Map<TeamDto>(team);

            return teamDto;
        }

        public async Task AddTeam(NewTeamDto team, string email)
        {
            var teamPlayers = new List<TeamPlayer>();
            var newUsersToSendInvitation = new List<TeamPlayerDto>();

            // Match players to existing users or create new ones
            foreach (var player in team.Players)
            {
                if (!string.IsNullOrEmpty(player.Email))
                {
                    var existingUser = await _userRepository.GetAll()
                                                            .Include(u => u.Credentials)
                                                            .FirstOrDefaultAsync(u => u.Credentials.Email == player.Email);

                    if (existingUser != null)
                    {
                        teamPlayers.Add(new TeamPlayer
                        {
                            Player = existingUser,
                            JoinDate = DateTime.UtcNow
                        });
                        continue;
                    }
                }

                // Add new user to the list to send an invitation
                newUsersToSendInvitation.Add(player);
                teamPlayers.Add(new TeamPlayer
                {
                    Player = new User
                    {
                        FirstName = player.FirstName,
                        LastName = player.LastName,
                        Height = (byte?)player.Height,
                        JerseyNumber = (byte?)player.JerseyNumber,
                        PositionId = 2,
                        Credentials = null // New user without credentials yet
                    },
                    JoinDate = DateTime.UtcNow
                });
            }

            // Find the captain by email
            var captainCredentials = await _credentialsRepository.GetAll()
                                           .Include(c => c.User)
                                           .FirstOrDefaultAsync(c => c.Email == email);

            if (captainCredentials?.User == null)
            {
                throw new Exception("Captain not found.");
            }

            // Create new team
            var newTeam = new Team
            {
                Name = team.Name,
                CreationDate = DateTime.UtcNow,
                Image = team.Image,
                LeagueId = 7,
                //    LeagueId = team.LeagueId, // Ensure this is a valid LeagueId
                CaptainId = captainCredentials.User.Id,
                TeamPlayers = teamPlayers,
                Email = team.Email,
                Logo = team.Logo,
                Phone = team.Phone,
                TeamDescription = team.TeamDescription,
                Website = team.Website,
                IsReportedToPlay = false
            };

            try
            {
                await _teamRepository.InsertAsync(newTeam);
                await _teamRepository.SaveChangesAsync();

                foreach (var player in teamPlayers)
                {
                    player.Team = newTeam;
                    await _teamRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while inserting the team: {ex}");
                throw;
            }

            // Send invitations to new users
            foreach (var newUser in newUsersToSendInvitation)
            {
              //  SendEmailAddedToTeam(newUser);
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

        public async Task<bool> UpdateExtendedTeam(ExtendedTeamDto extendedTeamDto)
        {
            if (extendedTeamDto == null) throw new ArgumentNullException(nameof(extendedTeamDto));

            try
            {
                var teamToUpdate = await _teamRepository.GetAll()
                    .FirstOrDefaultAsync(t => t.Id == extendedTeamDto.Id);
                if (teamToUpdate == null) return false;  // Zespół nie został znaleziony

                _mapper.Map(extendedTeamDto, teamToUpdate); // Mapuj zaktualizowane wartości na pobrany obiekt zespołu

                _teamRepository.Update(teamToUpdate); // Aktualizacja zespołu
                //await _teamRepository.SaveChangesAsync(); // Zapisz zmiany w bazie danych
                return true;
            }
            catch (Exception ex)
            {
                // Logowanie szczegółów wyjątku
                // Obsługa nieoczekiwanych wyjątków
                return false;
            }
        }


        //public async Task<bool> UpdateTeam(ManageTeamDto teamDto)
        //{
        //    var teamToUpdate = await _teamRepository.GetAll()
        //        .Include(u => u.Captain).ThenInclude(c => c.Credentials)
        //        .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player).ThenInclude(p => p.Credentials)
        //        .FirstOrDefaultAsync(t => t.Captain.Credentials.Email == "nowy@mail.com");

        //    if (teamToUpdate == null)
        //    {
        //        return false;
        //    }

        //    _mapper.Map(teamDto, teamToUpdate);

        //    var existingPlayerIds = teamToUpdate.TeamPlayers.Select(tp => tp.PlayerId).ToList();
        //    foreach (var playerDto in teamDto.Players.Where(p => existingPlayerIds.Contains((int)p.Id)))
        //    {
        //        var player = teamToUpdate.TeamPlayers.FirstOrDefault(tp => tp.PlayerId == playerDto.Id)?.Player;
        //        if (player != null)
        //        {
        //            _mapper.Map(playerDto, player);
        //        }
        //    }

        //    foreach (var newPlayerDto in teamDto.NewPlayers)
        //    {
        //        if (!existingPlayerIds.Contains((int)newPlayerDto.Id))
        //        {
        //            var newPlayer = _mapper.Map<User>(newPlayerDto);
        //            teamToUpdate.TeamPlayers.Add(new TeamPlayer { Player = newPlayer, JoinDate = DateTime.UtcNow });
        //        }
        //    }

        //    foreach (var player in teamDto.RemovedPlayers)
        //    {
        //        var playerToRemove = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Player.Id == player.Id);
        //        if (playerToRemove != null)
        //        {
        //            teamToUpdate.TeamPlayers.Remove(playerToRemove);
        //        }
        //    }

        //    try
        //    {
        //        await _teamRepository.UpdateAsync(teamToUpdate);
        //        await _teamRepository.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> UpdateTeam(ManageTeamDto team, string email)
        {

            var teamToUpdate = await _teamRepository.GetAll()
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (teamToUpdate == null)
            {
                return false;
            }

            teamToUpdate.Image = team.Photo;
            teamToUpdate.Email = team.Email;
            teamToUpdate.Logo = team.Logo;
            teamToUpdate.Phone = team.Phone;
            teamToUpdate.TeamDescription = team.TeamDescription;
            teamToUpdate.Website = team.Website;

            teamToUpdate.Captain.JerseyNumber = (byte?)team.Captain.JerseyNumber;
            teamToUpdate.Captain.Height = (byte?)team.Captain.Height;
            teamToUpdate.Captain.PositionId = team.Captain.PositionId;
            teamToUpdate.Captain.Gender = team.Captain.Gender;

            foreach (var player in team.Players)
            {
                var playerToUpdate = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Player.Id == player.Id);
                if (playerToUpdate != null)
                {
                    playerToUpdate.Player.JerseyNumber = (byte?)player.JerseyNumber;
                    playerToUpdate.Player.Height = (byte?)player.Height;
                    playerToUpdate.Player.PositionId = player.PositionId;
                    playerToUpdate.Player.Gender = player.Gender;
                    if (playerToUpdate.Player.Credentials == null)
                    {
                        playerToUpdate.Player.AdditionalEmail = player.Email;
                    }
                }
            }

            teamToUpdate.TeamPlayers.Concat(team.NewPlayers.Select(p => new TeamPlayer
            {
                Player = new User
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Height = (byte?)p.Height,
                    JerseyNumber = (byte?)p.JerseyNumber,
                    PositionId = p.PositionId,
                    Credentials = null
                },
                JoinDate = DateTime.Now
            }).ToList());

            foreach (var player in team.NewPlayers)
            {
                var user = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(c => c.Email == player.Email);
                if (user != null)
                {
                    teamToUpdate.TeamPlayers.Add(new TeamPlayer
                    {
                        Player = user.User,
                        JoinDate = DateTime.Now
                    });
                }
                else
                {
                    teamToUpdate.TeamPlayers.Add(new TeamPlayer
                    {
                        Player = new User
                        {
                            FirstName = player.FirstName,
                            LastName = player.LastName,
                            Height = (byte?)player.Height,
                            JerseyNumber = (byte?)player.JerseyNumber,
                            PositionId = player.PositionId,
                            Credentials = null,
                            AdditionalEmail = player.Email
                        },
                        JoinDate = DateTime.Now
                    });

                    if (player.Email != null)
                    {
                        //SendEmailAddedToTeam(player);
                    }
                }
            }

            foreach (var player in team.RemovedPlayers)
            {
                var playerid = player.Id;
                var playerToRemove = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Id == playerid);
                if (playerToRemove != null)
                {
                    teamToUpdate.TeamPlayers.Remove(playerToRemove);
                }
            }

            try
            {
                _teamRepository.Update(teamToUpdate);
                await _teamRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
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
                            .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Position)
                            .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            var result = _mapper.Map<ManagedTeamDataDto>(team);
            result.Logo = team?.Logo;
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

            foreach (var team in result)
            {
                team.Logo = teams.FirstOrDefault(t => t.Id == team.Id)?.Logo;
            }

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
