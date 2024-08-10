using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IEmailService _emailService;
        private readonly IBaseRepository<Team> _teamRepository;
        private readonly IBaseRepository<TeamPlayer> _teamPlayerRepository;
        private readonly IBaseRepository<TeamImage> _teamImageRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<League> _leagueRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;

        public TeamService(IMapper mapper, ILogService logService, IEmailService emailService, IWebHostEnvironment env, IBaseRepository<TeamPlayer> teamPlayerRepository, IBaseRepository<TeamImage> teamImageRepository, IBaseRepository<Team> teamRepository, IBaseRepository<League> leagueRepository, IBaseRepository<User> userRepository, IBaseRepository<Credentials> credentialsRepository)
        {
            _mapper = mapper;
            _logService = logService;
            _emailService = emailService;
            _teamRepository = teamRepository;
            _teamImageRepository = teamImageRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _env = env;
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
                .Include(t => t.TeamImage)
                .Include(t => t.League)
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (team == null)
            {
                return null;
            }

            TeamDto teamDto = _mapper.Map<TeamDto>(team);

            if (team.TeamImage != null) { 
                teamDto.Photo = team.TeamImage.Image;
        }
            return teamDto;
        }

        public async Task<(bool Success, string Message)> AddTeam(NewTeamDto team, string email)
        {
            var teamPlayers = new List<TeamPlayer>();
            var newUsersToSendInvitation = new List<TeamPlayerDto>();

            for (int i = 0; i < team.Players.Count; i++)
            {
                var player = team.Players[i];

                if (i == 0)
                {
                    var existingUser = await _userRepository.GetAll()
                                                                .Include(u => u.Credentials)
                                                                .Include(u => u.TeamPlayers)
                                                                .FirstOrDefaultAsync(u => u.Credentials.Email == player.Email);
                    teamPlayers.Add(new TeamPlayer
                    {
                        Player = existingUser,
                        JoinDate = DateTime.UtcNow
                    });
                }
                else
                {

                    if (!string.IsNullOrEmpty(player.Email))
                    {
                        var existingUser = await _userRepository.GetAll()
                                                                .Include(u => u.Credentials)
                                                                .Include(u => u.TeamPlayers)
                                                                .FirstOrDefaultAsync(u => u.Credentials.Email == player.Email);

                        if (existingUser != null)
                        {
                            if (existingUser.TeamPlayers.Any())
                            {
                                return (false, $"Użytkownik z adresem e-mail {player.Email} już istnieje i jest przypisany do drużyny.");
                            }

                            teamPlayers.Add(new TeamPlayer
                            {
                                Player = new User
                                {
                                    FirstName = player.FirstName,
                                    LastName = player.LastName,
                                    Height = (byte?)player.Height,
                                    JerseyNumber = (byte?)player.JerseyNumber,
                                    AdditionalEmail = player.Email,
                                    PositionId = 2,
                                    Credentials = null
                                },
                                JoinDate = DateTime.UtcNow
                            });

                            continue;
                        }

                        var userInUsersOnly = await _userRepository.GetAll()
                                                                   .Include(u => u.TeamPlayers)
                                                                   .FirstOrDefaultAsync(u => u.AdditionalEmail == player.Email);

                        if (userInUsersOnly != null)
                        {
                            if (userInUsersOnly.TeamPlayers.Any())
                            {
                                return (false, $"Użytkownik z adresem e-mail {player.Email} już istnieje i jest przypisany do drużyny.");
                            }

                            teamPlayers.Add(new TeamPlayer
                            {
                                Player = userInUsersOnly,
                                JoinDate = DateTime.UtcNow
                            });
                            continue;
                        }
                    }


                    newUsersToSendInvitation.Add(player);
                    teamPlayers.Add(new TeamPlayer
                    {
                        Player = new User
                        {
                            FirstName = player.FirstName,
                            LastName = player.LastName,
                            Height = (byte?)player.Height,
                            JerseyNumber = (byte?)player.JerseyNumber,
                            AdditionalEmail = player.Email,
                            PositionId = 2,
                            Credentials = null
                        },
                        JoinDate = DateTime.UtcNow
                    });

                    await SendEmailAddedToTeam(player, team.TeamDescription);
                }
            }

            var captainCredentials = await _credentialsRepository.GetAll()
                                           .Include(c => c.User)
                                           .FirstOrDefaultAsync(c => c.Email == email);

            if (captainCredentials?.User == null)
            {
                throw new Exception("Captain not found.");
            }

            var newTeam = new Team
            {
                Name = team.Name,
                CreationDate = DateTime.UtcNow,
                Image = team.Image,
                LeagueId = 7,
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
                Console.WriteLine($"An error occurred while inserting the team: {ex}");
                throw;
            }

            foreach (var newUser in newUsersToSendInvitation)
            {
                await SendEmailAddedToTeam(newUser, newTeam.Name);
            }

            return (true, "Drużyna została pomyślnie dodana.");
        }

        public async Task<(bool Success, string Message)> DeleteTeamImage(int teamId)
        {
            var teamImage = await _teamImageRepository.GetAll().FirstOrDefaultAsync(ti => ti.TeamId == teamId);
            if (teamImage == null)
            {
                return (false, "Image not found.");
            }

            await _teamImageRepository.Delete(teamImage);
            await _teamImageRepository.SaveChangesAsync();

            return (true, "Image deleted successfully.");
        }

        public async Task<(bool Success, string Message)> UploadTeamImage(int teamId, IFormFile file)
        {
            var team = await _teamRepository.GetAll().Include(t => t.TeamImage).FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                return (false, "Team not found.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var teamImage = new TeamImage
                {
                    TeamId = teamId,
                    Image = memoryStream.ToArray(),
                    ImageType = file.ContentType
                };

                Console.WriteLine($"Uploading image with MIME type: {file.ContentType}");

                if (team.TeamImage != null)
                {
                    team.TeamImage.Image = teamImage.Image;
                    team.TeamImage.ImageType = teamImage.ImageType;
                    _teamImageRepository.Update(team.TeamImage);
                }
                else
                {
                    await _teamImageRepository.InsertAsync(teamImage);
                }

                await _teamImageRepository.SaveChangesAsync();
            }

            return (true, "Image uploaded successfully.");
        }


        public async Task<TeamImage?> GetTeamImageByTeamId(int teamId)
        {
            return await _teamImageRepository.GetAll().FirstOrDefaultAsync(ti => ti.TeamId == teamId);
        }

        public async Task<List<TeamImageDto>> GetAllTeamsImagesStatus()
        {
            var teams = await _teamRepository.GetAll().ToListAsync();
            var teamImageDtos = teams.Select(team => new TeamImageDto
            {
                Id = team.Id,
                Name = team.Name,
                HasImage = TeamHasImage(team.Id)
            }).ToList();

            return teamImageDtos;
        }

        private bool TeamHasImage(int teamId)
        {
            return _teamImageRepository.GetAll().Any(ti => ti.TeamId == teamId);
        }

        public async Task<bool> DeleteTeam(int teamId)
        {
            var teamToDelete = await _teamRepository.GetAll().Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player).FirstOrDefaultAsync(t => t.Id == teamId);

            if (teamToDelete == null)
            {
                return false;
            }

            var usersToDelete = teamToDelete.GetAllPlayers().Where(u => !_credentialsRepository.GetAll().Any(c => c.UserId == u.Id)).ToList();

            try
            {
                foreach (var user in usersToDelete)
                {
                    await _userRepository.Delete(user);
                }

                await _userRepository.SaveChangesAsync();

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
                if (teamToUpdate == null) return false;  

                _mapper.Map(extendedTeamDto, teamToUpdate);

                _teamRepository.Update(teamToUpdate); 
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(bool Success, string Message)> UpdateTeam(ManageTeamDto team, string email)
        {
            var teamToUpdate = await _teamRepository.GetAll()
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (teamToUpdate == null)
            {
                return (false, "Nie znaleziono drużyny.");
            }

            var remainingPlayersCount = team.Players.Count + team.NewPlayers.Count - team.RemovedPlayers.Count;
            if (remainingPlayersCount < 6)
            {
                return (false, "Drużyna musi mieć co najmniej 6 zawodników.");
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
                var playerToUpdate = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Id == player.Id);
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

            foreach (var player in team.NewPlayers)
            {
                var newTeamPlayer = new TeamPlayer();
                var existingPlayer = await _userRepository.GetAll()
                    .Include(u => u.TeamPlayers)  
                    .FirstOrDefaultAsync(u => u.AdditionalEmail == player.Email);

                if (existingPlayer != null && existingPlayer.TeamPlayers.Any())
                {
                    return (false, $"Zawodnik z adresem email {player.Email} posiada już drużynę.");
                }

                    newTeamPlayer = new TeamPlayer
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
                    };

                    await SendEmailAddedToTeam(player, player.FirstName);
                
                teamToUpdate.TeamPlayers.Add(newTeamPlayer);
            }

            foreach (var player in team.RemovedPlayers)
            {
                var playerToRemove = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Id == player.Id);
                if (playerToRemove != null)
                {
                    teamToUpdate.TeamPlayers.Remove(playerToRemove);
                }
            }

            await _teamRepository.SaveChangesAsync();

            foreach (var player in team.NewPlayers)
            {
                var user = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(c => c.Email == player.Email);
                if (user != null)
                {
                    var newTeamPlayer = teamToUpdate.TeamPlayers.FirstOrDefault(tp => tp.Player.AdditionalEmail == player.Email);

                    if (newTeamPlayer != null)
                    {
                        await _logService.AddLog(player.FirstName + " " + player.LastName + " dołączył do drużyny " + team.TeamDescription, "user-profile/" + newTeamPlayer.Id, false, newTeamPlayer.Team.GetAllPlayers());
                    }
                }
                else
                {
                    var newTeamPlayer = teamToUpdate.TeamPlayers.FirstOrDefault(tp => tp.Player.AdditionalEmail == player.Email);

                    if (newTeamPlayer != null)
                    {
                        if (player.Email != null)
                        {
                            await SendEmailAddedToTeam(player, team.TeamDescription);
                        }

                        await _logService.AddLog(player.FirstName + " " + player.LastName + " dołączył do drużyny " + team.TeamDescription, "user-profile/" + newTeamPlayer.Id, false, /*newTeamPlayer.Team.GetAllPlayers()*/null);
                    }
                }
            }

            return (true, "Dane drużyny zostały pomyślnie zaktualizowane.");
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

        public async Task<bool> UpdateCaptain(string newCaptainId, string identity)
        {
            var team = await _teamRepository.GetAll()
                .Include(t => t.League)
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == identity 
                                    || t.Captain.Credentials.LoweredUserName == identity 
                                    || t.Captain.Credentials.UserName == identity);

            if (team == null)
            {
                return false;
            }

            var newCaptain = await _userRepository.GetAll().Include(z => z.Credentials)
                .FirstOrDefaultAsync(u => u.Credentials.Email == newCaptainId);

            var oldCaptainId = team.Captain.Id;

            if (newCaptain == null)
            {
                return false;
            }
            team.Captain = newCaptain;

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
        public async Task<(bool Success, string Message)> LeaveTeamByEmail(string email)
        {
            var user = await _userRepository.GetAll()
                                            .Include(u => u.TeamPlayers)
                                            .Include(u => u.Team)
                                            .FirstOrDefaultAsync(u => u.Credentials.Email == email);

            if (user == null)
            {
                return (false, "Użytkownik nie znaleziony.");
            }

            var isCaptain = await _teamRepository.GetAll()
                                                 .AnyAsync(t => t.CaptainId == user.Id);

            if (isCaptain)
            {
                return (false, "Kapitan nie może opuścić drużyny. Przekaż drużynę innemu kapitanowi i wtedy opuść drużynę.");
            }

            var teamPlayer = user.TeamPlayers.FirstOrDefault();
            if (teamPlayer == null)
            {
                return (false, "Użytkownik nie należy do żadnej drużyny.");
            }

            await _teamPlayerRepository.Delete(teamPlayer);

            try
            {
                await _teamPlayerRepository.SaveChangesAsync();
                return (true, "Pomyślnie opuściłeś drużynę.");
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas opuszczania drużyny: {ex.Message}");
            }
        }

        public async Task<Match?> GetClosestMatch(int teamId)
        {
            var team = await _teamRepository.GetAll()
                                            .Include(t => t.HomeMatches)
                                            .Include(t => t.GuestMatches)
                                            .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                return null;
            }

            var allMatches = team.HomeMatches.Concat(team.GuestMatches)
                                             .Where(m => m.CreationDate >= DateTime.Now)
                                             .OrderBy(m => m.CreationDate);

            return allMatches.FirstOrDefault();
        }

        private async Task SendEmailAddedToTeam(TeamPlayerDto newUser, string teamName)
        {
            string resourcePath = "VolleyLeague.Services.EmailTemplates.NewTeamMate.html";

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Email template file not found: {resourcePath}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string emailTemplate = await reader.ReadToEndAsync();
                    string emailBody = emailTemplate.Replace("TeamNameId", teamName);

                    var message = new MailMessage("noreply@volleyleague.com", newUser.Email)
                    {
                        Subject = "Welcome to the Team!",
                        Body = emailBody,
                        IsBodyHtml = true,
                    };

                    await _emailService.Send(newUser.Email, message);
                }
            }
        }

    }
}
