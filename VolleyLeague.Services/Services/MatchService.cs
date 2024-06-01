using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using VolleyLeague.Entities.Dtos.Matches;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Repositories.Repositories;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class MatchService : IMatchService
    {
        private readonly ILogger<SeasonService> _logger;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Match> _matchRepository;
        private readonly IBaseRepository<Team> _teamRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IRoleRepository _roleRepository;
        public MatchService(
            IMapper mapper,
            ILogService logService,
            IBaseRepository<Match> matchRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<Team> teamRepository,
            IRoleRepository roleRepository
            )
        {
            _mapper = mapper;
            _logService = logService;
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
         }

        public async Task<List<MatchSummaryDto>> GetAllMatchesAsync()
        {
            var matches = (await _matchRepository.GetAll()
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.Mvp)
                .ToListAsync());

            var matchesDto = _mapper.Map<List<MatchSummaryDto>>( matches );

            return matchesDto;
        }

        public async Task<bool> DeleteMatch(int id)
        {
            var match = await _matchRepository.GetAll().FirstOrDefaultAsync(m => m.Id == id);
            await _matchRepository.Delete(match);
            await _matchRepository.SaveChangesAsync();
            return true;
        }

        public async Task<MatchDto> GetMatchByIdAsync(int id)
        {
            var match = await _matchRepository.GetAll()
                .Include(m => m.League)
                .Include(m => m.GuestTeam).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
                .Include(m => m.HomeTeam).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
                .Include(m => m.Mvp)
                .Include(m => m.Round)
                .Include(m => m.Venue)
                .Include(m => m.Referee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return null;
            }

            var result = _mapper.Map<MatchDto>( match );
            return result;
        }

        public async Task<List<PlayerSummaryDto>> GetReferees()
        {
            List<User> referees = await _userRepository.GetAll()
                .Include(p => p.Team)
                .Include(p => p.Credentials)
                .Where(p => p.Credentials != null)
                .Include(p => p.Credentials)
                .ThenInclude(c => c!.Roles)
                .Where(p => p.Credentials!.Roles.Any(r => r.Name == Roles.Arbiter))
                .ToListAsync();

            var response = _mapper.Map<List<PlayerSummaryDto>>(referees);

            return response;
        }

        public async Task<bool> RemoveReferee(int userId)
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.Credentials)
                .ThenInclude(c => c.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Credentials == null)
            {
                return false;
            }

            var roleToRemove = user.Credentials.Roles.FirstOrDefault(r => r.Name == Roles.Arbiter);
            if (roleToRemove == null)
            {
                return false;
            }

            user.Credentials.Roles.Remove(roleToRemove);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<PlayerSummaryDto>> GetPotentialReferees()
        {
            List<User> referees = await _userRepository.GetAll()
                .Include(p => p.Team)
                .Include(p => p.Credentials)
                .Where(p => p.Credentials != null)
                .Include(p => p.Credentials)
                .ThenInclude(c => c!.Roles)
                .Where(p => !p.Credentials!.Roles.Any(r => r.Name == Roles.Arbiter))
                .ToListAsync();

            var response = _mapper.Map<List<PlayerSummaryDto>>(referees);

            return response;
        }

        public async Task<bool> AddReferee(int userId)
        {
            var user = await _userRepository.GetAll()
                .Include(p => p.Credentials)
                .ThenInclude(t => t!.Roles)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user != null && user.Credentials != null)
            {
                var hasArbiterRole = user.Credentials.Roles.Any(r => r.Name == Roles.Arbiter);
                if (!hasArbiterRole)
                {
                    var arbiterRoles = await _roleRepository.GetRoles(); 
                    var arbiterRole = arbiterRoles.FirstOrDefault(r => r.Name == Roles.Arbiter);
                    if (arbiterRole != null)
                    {
                        user.Credentials.Roles.Add(arbiterRole);
                        await _userRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<List<PlayerSummaryDto>> GetOtherData()
        {
            List<User> other = await _userRepository.GetAll()
                .Where(p => p.Id == 8499)
                .Include(p => p.Team)
                .Include(p => p.Credentials)
                .ThenInclude(c => c!.Roles)
                .ToListAsync();
            
            var response = _mapper.Map<List<PlayerSummaryDto>>(other);

            return response;
        }

        public async Task<List<MatchSummaryDto>> GetMatchesByLeagueIdAsync(int id)
        {
            var matches = (await _matchRepository.GetAll()
                               .Include(m => m.League)
                               .Include(m => m.HomeTeam)
                               .Include(m => m.Mvp)
                               .Where(m => m.LeagueId == id)
                               .ToListAsync());

            var response = _mapper.Map<List<MatchSummaryDto>>( matches );
            return response;
        }

        public async Task<List<MatchSummaryDto>> GetMatches(int seasonId, int teamId)
        {
            var matches = await _matchRepository.GetAll()
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Mvp)
                .Include(m => m.Referee)
                .Include(m => m.Venue)
                .Include(m => m.Round)
                .Where(m => m.Round.SeasonId == seasonId && (m.HomeTeamId == teamId || m.GuestTeamId == teamId))
                .ToListAsync();

            var result = _mapper.Map<List<MatchSummaryDto>>( matches );
            return result;
        }

        public async Task<List<MatchSummaryDto>> GetLast10Matches()
        {
            var matches = await _matchRepository.GetAll()
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Mvp)
                .Include(m => m.Referee)
                .Include(m => m.Venue)
                .Include(m => m.Round)
                .OrderByDescending(m => m.Schedule)
                .Take(10)
                .ToListAsync();

            var result = _mapper.Map<List<MatchSummaryDto>>(matches);
            return result;
        }

        public async Task<List<MatchSummaryDto>> GetMatches(int leagueId, int seasonId, int roundId)
        {
            var matches = await _matchRepository.GetAll()
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Mvp)
                .Include(m => m.Referee)
                .Include(m => m.Venue)
                .Include(m => m.Round)
                .Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId && m.RoundId == roundId)
                .ToListAsync();

            var result = _mapper.Map<List<MatchSummaryDto>>(matches);

            foreach (var team in result)
            {
                var match = matches.FirstOrDefault(t => t.Id == team.Id);
                if (match?.GuestTeam.Logo != null && match.GuestTeam.Logo.Length > 0)
                {
                    team.GuestTeam.Logo = match.GuestTeam.Logo; // Przypisujemy logo, jeśli spełniono warunki
                }
            }

            foreach (var team in result)
            {
                var match = matches.FirstOrDefault(t => t.Id == team.Id);
                if (match?.HomeTeam.Logo != null && match.HomeTeam.Logo.Length > 0)
                {
                    team.HomeTeam.Logo = match.HomeTeam.Logo; // Przypisujemy logo, jeśli spełniono warunki
                }
            }


            return result;
        }

        //public async Task<List<MatchDto>> GetLast10Matches()
        //{
        //    var last10Matches = await _matchRepository.GetAll()
        //        .Include(m => m.League)
        //        .Include(m => m.Round)
        //            .ThenInclude(r => r.Season)
        //        .Include(m => m.HomeTeam) 
        //        .Include(m => m.GuestTeam) 
        //        .OrderByDescending(m => m.Schedule) 
        //        .Take(10)
        //        .ToListAsync();

        //    var matchesDto = new List<MatchDto>(); 

        //    foreach (var match in last10Matches)
        //    {
        //        var matchDto = _mapper.Map<MatchDto>(match);
        //        matchesDto.Add(matchDto);
        //    }

        //    return matchesDto;
        //}

        public async Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId)
        {
            var matchesDto = await _teamRepository.GetAll()
                .Include(m => m.League)
                .Include(m => m.GuestMatches)
                    .ThenInclude(m => m.Round)
                    .ThenInclude(r => r.Season)
                .Include(m => m.HomeMatches)
                    .ThenInclude(m => m.Round)
                    .ThenInclude(r => r.Season)
                .Where(m => m.LeagueId == leagueId)
                .ToListAsync();

            List<StandingsDto> standings = new List<StandingsDto>();

            foreach (var team in matchesDto)
            {
                StandingsDto standingsDto = new StandingsDto();
                standingsDto.Team = _mapper.Map<TeamSummaryDto>(team);
                standingsDto.Team.Logo = team.Logo; 
                standingsDto.Team.Id = team.Id;
                standingsDto.Team.Name = team.Name;
                standingsDto.Team.LeagueId = team.LeagueId;
                List<Match> matchesHome = team.HomeMatches.Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId).ToList();
                List<Match> matchesGuest = team.GuestMatches.Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId).ToList();
                List<Match> combinedMatches = matchesHome.Concat(matchesGuest).ToList();
                IEnumerable<Match> matches = team.HomeMatches.Concat(team.GuestMatches).Where(m => m.Round.SeasonId == seasonId);
                standingsDto.MatchesPlayed = combinedMatches.Count();
                standingsDto.MatchesWon = CalculateTotalHostWins(matchesHome) + CalculateTotalGuestWins(matchesGuest);
                standingsDto.MatchesWon = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score > m.Team2Score || m.GuestTeamId == team.Id && m.Team2Score > m.Team1Score);
                standingsDto.MatchesLost = standingsDto.MatchesPlayed - standingsDto.MatchesWon;
                standingsDto.SetsWon = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team1Score : m.Team2Score);
                standingsDto.SetsLost = matches.Sum(m => m.HomeTeamId == team.Id ? m.Team2Score : m.Team1Score);
                standingsDto.PointsWon = matches.Sum(m => m.HomeTeamId == team.Id ?
                (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0) :
                (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0));
                standingsDto.PointsLost = matches.Sum(m => m.HomeTeamId == team.Id ?
                    (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0) :
                    (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0));
                standingsDto.SetsRatio = (double)standingsDto.SetsWon / (double)(standingsDto.SetsLost == 0 ? 1 : standingsDto.SetsLost);
                standingsDto.BallsRatio = (double)standingsDto.PointsWon / (double)(standingsDto.PointsLost == 0 ? 1 : standingsDto.PointsLost);
                standingsDto.Score3_0 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 0 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 0);
                standingsDto.Score3_1 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 1 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 1);
                standingsDto.Score3_2 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 2 || m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 2);
                standingsDto.Score2_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 2 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 2 && m.Team1Score == 3);
                standingsDto.Score1_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 1 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 1 && m.Team1Score == 3);
                standingsDto.Score0_3 = matches.Count(m => m.HomeTeamId == team.Id && m.Team1Score == 0 && m.Team2Score == 3 || m.GuestTeamId == team.Id && m.Team2Score == 0 && m.Team1Score == 3);
                standingsDto.Points = CalculateVolleyballPoints(matchesHome, matchesGuest);
                //var teamStandings = _mapper.Map<StandingsDto>(team);
                standings.Add(standingsDto);
            }

            var standingsToReturn = standings.OrderByDescending(s => s.Points).ThenByDescending(s => s.SetsRatio).ThenByDescending(s => s.BallsRatio).ToList();

            return standingsToReturn;
        }

        private int CalculateTotalHostWins(IEnumerable<Match> matches)
        {
            int totalHostWins = 0;

            foreach (var match in matches)
            {
                int hostWins = 0;
                int guestWins = 0;

                if (match.Set1Team1Score > match.Set1Team2Score) hostWins++;
                if (match.Set2Team1Score > match.Set2Team2Score) hostWins++;
                if (match.Set3Team1Score > match.Set3Team2Score) hostWins++;
                if (match.Set4Team1Score > match.Set4Team2Score) hostWins++;
                if (match.Set5Team1Score > match.Set5Team2Score) hostWins++;

                if (match.Set1Team2Score > match.Set1Team1Score) guestWins++;
                if (match.Set2Team2Score > match.Set2Team1Score) guestWins++;
                if (match.Set3Team2Score > match.Set3Team1Score) guestWins++;
                if (match.Set4Team2Score > match.Set4Team1Score) guestWins++;
                if (match.Set5Team2Score > match.Set5Team1Score) guestWins++;

                if (hostWins > guestWins) totalHostWins++;
            }

            return totalHostWins;
        }

        private int CalculateTotalGuestWins(IEnumerable<Match> matches)
        {
            int totalGuestWins = 0;

            foreach (var match in matches)
            {
                int hostWins = 0;
                int guestWins = 0;

                if (match.Set1Team1Score > match.Set1Team2Score) hostWins++;
                if (match.Set2Team1Score > match.Set2Team2Score) hostWins++;
                if (match.Set3Team1Score > match.Set3Team2Score) hostWins++;
                if (match.Set4Team1Score > match.Set4Team2Score) hostWins++;
                if (match.Set5Team1Score > match.Set5Team2Score) hostWins++;

                if (match.Set1Team2Score > match.Set1Team1Score) guestWins++;
                if (match.Set2Team2Score > match.Set2Team1Score) guestWins++;
                if (match.Set3Team2Score > match.Set3Team1Score) guestWins++;
                if (match.Set4Team2Score > match.Set4Team1Score) guestWins++;
                if (match.Set5Team2Score > match.Set5Team1Score) guestWins++;

                if (guestWins > hostWins) totalGuestWins++;
            }

            return totalGuestWins;
        }

        private int CalculateVolleyballPoints(IEnumerable<Match> hostMatches, IEnumerable<Match> guestMatches)
        {
            int totalPoints = 0;

            // Funkcja pomocnicza do obliczania punktów za pojedynczy mecz
            int CalculateMatchPoints(int wins, int losses, bool isHost)
            {
                if (isHost)
                {
                    // Jako gospodarz
                    if (wins == 3 && (losses == 0 || losses == 1))
                    {
                        return 3; // 3 punkty za wygraną 3:0 lub 3:1
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; // 2 punkty za wygraną 3:2
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; // 1 punkt za przegraną 2:3
                    }
                }
                else
                {
                    // Jako gość
                    if ((losses == 0 || losses == 1) && wins == 3)
                    {
                        return 3; // 3 punkty za przegraną 0:3 lub 1:3
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; // 2 punkty za przegraną 2:3
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; // 1 punkt za wygraną 3:2
                    }
                }

                // W przypadku innych wyników 0 punktów
                return 0;
            }

            // Zliczanie punktów za mecze jako gospodarz
            foreach (var match in hostMatches)
            {
                int hostWins = 0;
                int guestWins = 0;

                // Sprawdzanie wygranych setów
                if (match.Set1Team1Score > match.Set1Team2Score) hostWins++;
                if (match.Set2Team1Score > match.Set2Team2Score) hostWins++;
                if (match.Set3Team1Score > match.Set3Team2Score) hostWins++;
                if (match.Set4Team1Score > match.Set4Team2Score) hostWins++;
                if (match.Set5Team1Score > match.Set5Team2Score) hostWins++;

                if (match.Set1Team2Score > match.Set1Team1Score) guestWins++;
                if (match.Set2Team2Score > match.Set2Team1Score) guestWins++;
                if (match.Set3Team2Score > match.Set3Team1Score) guestWins++;
                if (match.Set4Team2Score > match.Set4Team1Score) guestWins++;
                if (match.Set5Team2Score > match.Set5Team1Score) guestWins++;

                totalPoints += CalculateMatchPoints(hostWins, guestWins, true);
            }

            // Zliczanie punktów za mecze jako gość
            foreach (var match in guestMatches)
            {
                int hostWins = 0;
                int guestWins = 0;

                // Sprawdzanie wygranych setów
                if (match.Set1Team2Score > match.Set1Team1Score) guestWins++;
                if (match.Set2Team2Score > match.Set2Team1Score) guestWins++;
                if (match.Set3Team2Score > match.Set3Team1Score) guestWins++;
                if (match.Set4Team2Score > match.Set4Team1Score) guestWins++;
                if (match.Set5Team2Score > match.Set5Team1Score) guestWins++;

                if (match.Set1Team1Score > match.Set1Team2Score) hostWins++;
                if (match.Set2Team1Score > match.Set2Team2Score) hostWins++;
                if (match.Set3Team1Score > match.Set3Team2Score) hostWins++;
                if (match.Set4Team1Score > match.Set4Team2Score) hostWins++;
                if (match.Set5Team1Score > match.Set5Team2Score) hostWins++;

                totalPoints += CalculateMatchPoints(guestWins, hostWins, false);
            }

            return totalPoints;
        }



        public async Task AddMatch(NewMatchDto match)
        {
            var newMatchEntity = _mapper.Map<Match>(match);
            await _matchRepository.InsertAsync(newMatchEntity);
            await _matchRepository.SaveChangesAsync();
            var homeTeam = _teamRepository.GetAll().Where(x => x.Id == match.HomeTeamId).FirstOrDefault();
            var guestTeam = _teamRepository.GetAll().Where(x => x.Id == match.GuestTeamId).FirstOrDefault();

            string logMessage = $"Mecz {homeTeam.Name} vs {guestTeam.Name} odbędzie się {newMatchEntity.Schedule.ToString("D")}";
            string logLink = $"match/" + newMatchEntity.Id;
            await _logService.AddLog(logMessage, logLink, false, null);
        }

        //public async Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId)
        //{
        //    var teams = await _matchRepository.GetAll()
        //        .Include(t => t.League)
        //        .Include(t => t.GuestMatches)
        //            .ThenInclude(m => m.Round)
        //            .ThenInclude(r => r.Season)
        //        .Include(t => t.HomeMatches)
        //            .ThenInclude(m => m.Round)
        //            .ThenInclude(r => r.Season)
        //        .Where(t => t.LeagueId == leagueId)
        //        .ToListAsync();

        //    response.Data = teams.Select(t => new StandingsDto(t, seasonId)).OrderByDescending(s => s.Points).ToList();

        //    return response;
        //}

        public async Task<List<PlayerSummaryDto>> GetMvpBySeasonAndLeague(int seasonId, int leagueId)
        {
            var mvpUsers = await _matchRepository.GetAll()
                .Include(m => m.Mvp)
                .Include(m => m.League)
                .Include(m => m.Round)
                .Where(m => m.LeagueId == leagueId && m.Round.SeasonId == seasonId)
                .GroupBy(m => m.Mvp)
                .Select(g => new { User = g.Key, TotalMvpCount = g.Count() })
                .OrderByDescending(x => x.TotalMvpCount)
                .ToListAsync();

            if (!mvpUsers.Any())
            {
                return new List<PlayerSummaryDto>();
            }

            //var result = mvpUsers.Select(mvp => new PlayerSummaryDto
            //{
            //    Id = mvp.User.Id,
            //    FirstName = mvp.User.FirstName,
            //    LastName = mvp.User.LastName,
            //    TotalMvpCount = mvp.TotalMvpCount 
            //}).ToList();
            var result = new List<PlayerSummaryDto>();

            if (mvpUsers.Any())
            {
                // Use foreach to populate the result list
                foreach (var mvp in mvpUsers)
                {
                    if (mvp.User != null)
                    {
                        var playerSummary = new PlayerSummaryDto
                        {
                            Id = mvp.User.Id,
                            FirstName = mvp.User.FirstName,
                            LastName = mvp.User.LastName,
                            TotalMvpCount = mvp.TotalMvpCount // Assign the MVP count
                        };

                        result.Add(playerSummary);
                    }
                }
            }

            return result;
        }


    }
}