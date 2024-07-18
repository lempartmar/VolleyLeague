using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;
using VolleyLeague.Shared.Dtos.Teams;

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
        private readonly IBaseRepository<TeamPlayer> _teamPlayerRepository;
        private readonly IRoleRepository _roleRepository;
        public MatchService(
            IMapper mapper,
            ILogService logService,
            IBaseRepository<Match> matchRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<Team> teamRepository,
            IBaseRepository<TeamPlayer> teamPlayerRepository,
            IRoleRepository roleRepository
            )
        {
            _mapper = mapper;
            _logService = logService;
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _teamPlayerRepository = teamPlayerRepository;
        }

        public async Task<List<MatchSummaryDto>> GetAllMatchesAsync()
        {
            var matches = (await _matchRepository.GetAll()
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.Mvp)
                .ToListAsync());

            var matchesDto = _mapper.Map<List<MatchSummaryDto>>(matches);

            return matchesDto;
        }

        public async Task<bool> DeleteMatch(int id)
        {
            var match = await _matchRepository.GetAll().FirstOrDefaultAsync(m => m.Id == id);
            await _matchRepository.Delete(match);
            await _matchRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<Match>> GetMatchesByRoundId(int roundId)
        {
            return await _matchRepository.GetAll()
                .Where(m => m.RoundId == roundId)
                .ToListAsync();
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

            var result = _mapper.Map<MatchDto>(match);
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

            var response = _mapper.Map<List<MatchSummaryDto>>(matches);
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

            var result = _mapper.Map<List<MatchSummaryDto>>(matches);
            return result;
        }

        public async Task<List<StandingsDto>> GetStandings(int seasonId, int leagueId)
        {
            var stopwatch = Stopwatch.StartNew();

            // Fetch the matches with necessary fields only
            var matches = await _matchRepository.GetAll()
                .AsNoTracking()
                .Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId)
                .Select(m => new MatchForStandingsDto
                {
                    HomeTeamId = m.HomeTeamId,
                    GuestTeamId = m.GuestTeamId,
                    Team1Score = m.Team1Score,
                    Team2Score = m.Team2Score,
                    Set1Team1Score = m.Set1Team1Score,
                    Set1Team2Score = m.Set1Team2Score,
                    Set2Team1Score = m.Set2Team1Score,
                    Set2Team2Score = m.Set2Team2Score,
                    Set3Team1Score = m.Set3Team1Score,
                    Set3Team2Score = m.Set3Team2Score,
                    Set4Team1Score = m.Set4Team1Score,
                    Set4Team2Score = m.Set4Team2Score,
                    Set5Team1Score = m.Set5Team1Score,
                    Set5Team2Score = m.Set5Team2Score,
                    HomeTeam = new TeamSummaryDto
                    {
                        Id = m.HomeTeam.Id,
                        Name = m.HomeTeam.Name,
                        Logo = m.HomeTeam.Logo
                    },
                    GuestTeam = new TeamSummaryDto
                    {
                        Id = m.GuestTeam.Id,
                        Name = m.GuestTeam.Name,
                        Logo = m.GuestTeam.Logo
                    }
                }).ToListAsync();

            stopwatch.Stop();
            Console.WriteLine($"Query Execution Time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            // Get distinct teams from matches
            var teamDictionary = new Dictionary<int, TeamSummaryDto>();
            foreach (var match in matches)
            {
                if (!teamDictionary.ContainsKey(match.HomeTeam.Id))
                {
                    teamDictionary.Add(match.HomeTeam.Id, match.HomeTeam);
                }
                if (!teamDictionary.ContainsKey(match.GuestTeam.Id))
                {
                    teamDictionary.Add(match.GuestTeam.Id, match.GuestTeam);
                }
            }
            var teams = teamDictionary.Values.ToList();

            var standings = new List<StandingsDto>();

            foreach (var team in teams)
            {
                var matchesHome = matches.Where(m => m.HomeTeamId == team.Id).ToList();
                var matchesGuest = matches.Where(m => m.GuestTeamId == team.Id).ToList();
                var combinedMatches = matchesHome.Concat(matchesGuest).ToList();

                var setsWon = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? m.Team1Score : m.Team2Score);
                var setsLost = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? m.Team2Score : m.Team1Score);
                var pointsWon = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0) : (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0));
                var pointsLost = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0) : (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0));

                var standingsDto = new StandingsDto
                {
                    Team = team,
                    MatchesPlayed = combinedMatches.Count,
                    MatchesWon = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score > m.Team2Score) || (m.GuestTeamId == team.Id && m.Team2Score > m.Team1Score)),
                    MatchesLost = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score < m.Team2Score) || (m.GuestTeamId == team.Id && m.Team2Score < m.Team1Score)),
                    SetsWon = setsWon,
                    SetsLost = setsLost,
                    PointsWon = pointsWon,
                    PointsLost = pointsLost,
                    SetsRatio = (double)setsWon / (setsLost == 0 ? 1 : setsLost),
                    BallsRatio = (double)pointsWon / (pointsLost == 0 ? 1 : pointsLost),
                    Score3_0 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 0) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 0)),
                    Score3_1 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 1) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 1)),
                    Score3_2 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 2) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 2)),
                    Score2_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 2 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 2 && m.Team1Score == 3)),
                    Score1_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 1 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 1 && m.Team1Score == 3)),
                    Score0_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 0 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 0 && m.Team1Score == 3)),
                    Points = CalculateVolleyballPoints(matchesHome, matchesGuest)
                };

                standings.Add(standingsDto);
            }

            stopwatch.Stop();
            Console.WriteLine($"Processing Time: {stopwatch.ElapsedMilliseconds} ms");

            return standings.OrderByDescending(s => s.Points)
                            .ThenByDescending(s => s.SetsRatio)
                            .ThenByDescending(s => s.BallsRatio)
                            .ToList();
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
        public async Task<List<StandingsDto>> GetStandingsBeforeOptimalization(int seasonId, int leagueId)
        {
            var matches = await _matchRepository.GetAll()
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Round)
                .Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId)
                .ToListAsync();

            var teams = matches.SelectMany(m => new[] { m.HomeTeam, m.GuestTeam }).Distinct();
            var standings = new List<StandingsDto>();

            foreach (var team in teams)
            {
                var matchesHome = matches.Where(m => m.HomeTeamId == team.Id).ToList();
                var matchesGuest = matches.Where(m => m.GuestTeamId == team.Id).ToList();
                var combinedMatches = matchesHome.Concat(matchesGuest).ToList();

                var setsWon = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? m.Team1Score : m.Team2Score);
                var setsLost = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? m.Team2Score : m.Team1Score);
                var pointsWon = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0) : (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0));
                var pointsLost = combinedMatches.Sum(m => m.HomeTeamId == team.Id ? (m.Set1Team2Score ?? 0) + (m.Set2Team2Score ?? 0) + (m.Set3Team2Score ?? 0) + (m.Set4Team2Score ?? 0) + (m.Set5Team2Score ?? 0) : (m.Set1Team1Score ?? 0) + (m.Set2Team1Score ?? 0) + (m.Set3Team1Score ?? 0) + (m.Set4Team1Score ?? 0) + (m.Set5Team1Score ?? 0));

                var standingsDto = new StandingsDto
                {
                    Team = _mapper.Map<TeamSummaryDto>(team),
                    MatchesPlayed = combinedMatches.Count(),
                    MatchesWon = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score > m.Team2Score) || (m.GuestTeamId == team.Id && m.Team2Score > m.Team1Score)),
                    MatchesLost = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score < m.Team2Score) || (m.GuestTeamId == team.Id && m.Team2Score < m.Team1Score)),
                    SetsWon = setsWon,
                    SetsLost = setsLost,
                    PointsWon = pointsWon,
                    PointsLost = pointsLost,
                    SetsRatio = (double)setsWon / (setsLost == 0 ? 1 : setsLost),
                    BallsRatio = (double)pointsWon / (pointsLost == 0 ? 1 : pointsLost),
                    Score3_0 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 0) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 0)),
                    Score3_1 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 1) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 1)),
                    Score3_2 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 3 && m.Team2Score == 2) || (m.GuestTeamId == team.Id && m.Team2Score == 3 && m.Team1Score == 2)),
                    Score2_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 2 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 2 && m.Team1Score == 3)),
                    Score1_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 1 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 1 && m.Team1Score == 3)),
                    Score0_3 = combinedMatches.Count(m => (m.HomeTeamId == team.Id && m.Team1Score == 0 && m.Team2Score == 3) || (m.GuestTeamId == team.Id && m.Team2Score == 0 && m.Team1Score == 3)),
                    Points = CalculateVolleyballPoints(matchesHome, matchesGuest)
                };

                standingsDto.Team.Logo = team.Logo;

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

            int CalculateMatchPoints(int wins, int losses, bool isHost)
            {
                if (isHost)
                {
                    if (wins == 3 && (losses == 0 || losses == 1))
                    {
                        return 3; 
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; 
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; 
                    }
                }
                else
                {

                    if ((losses == 0 || losses == 1) && wins == 3)
                    {
                        return 3; 
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; 
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; 
                    }
                }

                return 0;
            }

            foreach (var match in hostMatches)
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

                totalPoints += CalculateMatchPoints(hostWins, guestWins, true);
            }


            foreach (var match in guestMatches)
            {
                int hostWins = 0;
                int guestWins = 0;

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

        public async Task<List<NextMatchMinDto>> GetNextTwoMatchesAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            var matches = await _matchRepository.GetAll()
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Where(m => m.Schedule > DateTime.Now)
                .OrderBy(m => m.Schedule)
                .Take(2)
                .Select(m => new NextMatchMinDto
                {
                    Id = m.Id,
                    HomeTeam = new TeamSummaryDto
                    {
                        Id = m.HomeTeam.Id,
                        Name = m.HomeTeam.Name,
                        Logo = m.HomeTeam.Logo
                    },
                    GuestTeam = new TeamSummaryDto
                    {
                        Id = m.GuestTeam.Id,
                        Name = m.GuestTeam.Name,
                        Logo = m.GuestTeam.Logo
                    },
                    Schedule = m.Schedule
                })
                .ToListAsync();

            stopwatch.Stop();
            Console.WriteLine($"Query Execution Time: {stopwatch.ElapsedMilliseconds} ms");

            return matches;
        }

        public async Task<LastMatchDto> GetLastMatchAsync()
        {
            var match = await _matchRepository.GetAll()
                .AsNoTracking()
                .Where(m => m.Schedule <= DateTime.Now && m.Set1Team1Score != null)
                .OrderByDescending(m => m.Schedule)
                .Select(m => new LastMatchDto
                {
                    Id = m.Id,
                    HomeTeam = new TeamSummaryDto
                    {
                        Id = m.HomeTeam.Id,
                        Name = m.HomeTeam.Name,
                        Logo = m.HomeTeam.Logo
                    },
                    GuestTeam = new TeamSummaryDto
                    {
                        Id = m.GuestTeam.Id,
                        Name = m.GuestTeam.Name,
                        Logo = m.GuestTeam.Logo
                    },
                    Team1Score = m.Team1Score,
                    Team2Score = m.Team2Score,
                    Schedule = m.Schedule,
                    MatchInfo = m.MatchInfo
                })
                .FirstOrDefaultAsync();

            return match;
        }


        public async Task AddMatch(NewMatchDto match)
        {
            var newMatchEntity = _mapper.Map<Match>(match);
            newMatchEntity.CreationDate = DateTime.Now;
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
            // Pierwsze zapytanie: Uzyskanie użytkowników MVP
            var mvpUsers = await _matchRepository.GetAll()
                .Include(m => m.Mvp)
                .Include(m => m.League)
                .Include(m => m.Round)
                .Where(m => m.LeagueId == leagueId && m.Round.SeasonId == seasonId && m.MvpId != 32 && m.MvpId != 39)
                .GroupBy(m => m.Mvp)
                .Select(g => new { User = g.Key, TotalMvpCount = g.Count() })
                .OrderByDescending(x => x.TotalMvpCount)
                .ToListAsync();

            if (!mvpUsers.Any())
            {
                return new List<PlayerSummaryDto>();
            }

            var userIds = mvpUsers
                .Where(mvp => mvp.User != null) // Filtruj tylko tych, którzy mają User nie null
                .Select(mvp => mvp.User.Id)
                .ToList();

            // Lista do przechowywania informacji o drużynach dla użytkowników MVP
            var userTeams = new List<TeamPlayer>();

            // Pojedyncze zapytania dla każdego userId
            foreach (var userId in userIds)
            {
                var userTeam = await _teamPlayerRepository.GetAll()
                    .Where(tp => tp.PlayerId == userId)
                    .Include(tp => tp.Team)
                    .FirstOrDefaultAsync();

                if (userTeam != null)
                {
                    userTeams.Add(userTeam);
                }
            }

            // Utworzenie mapy użytkowników MVP i ich drużyn
            var teamDict = userTeams
                .Where(tp => tp.Team != null) // Tylko ci, którzy mają przypisany zespół
                .GroupBy(tp => tp.PlayerId)
                .ToDictionary(g => g.Key, g => g.First().Team.Name);

            // Stworzenie listy wynikowej, uwzględniającej brak drużyny
            var result = mvpUsers
                .Where(mvp => mvp.User != null)
                .Select(mvp => new PlayerSummaryDto
                {
                    Id = mvp.User.Id,
                    FirstName = mvp.User.FirstName,
                    LastName = mvp.User.LastName,
                    TotalMvpCount = mvp.TotalMvpCount,
                    TeamName = teamDict.TryGetValue(mvp.User.Id, out var teamName) ? teamName : "Brak drużyny"
                }).ToList();

            // Sprawdzenie, czy każdy z użytkowników ma drużynę
            foreach (var player in result)
            {
                Console.WriteLine($"Player: {player.FirstName} {player.LastName}, Team: {player.TeamName}");
            }

            return result;
        }

        private int CalculateVolleyballPoints(IEnumerable<MatchForStandingsDto> hostMatches, IEnumerable<MatchForStandingsDto> guestMatches)
        {
            int totalPoints = 0;

            // Helper function to calculate points for a single match
            int CalculateMatchPoints(int wins, int losses, bool isHost)
            {
                if (isHost)
                {
                    // As a host
                    if (wins == 3 && (losses == 0 || losses == 1))
                    {
                        return 3; // 3 points for a win 3:0 or 3:1
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; // 2 points for a win 3:2
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; // 1 point for a loss 2:3
                    }
                }
                else
                {
                    // As a guest
                    if ((losses == 0 || losses == 1) && wins == 3)
                    {
                        return 3; // 3 points for a loss 0:3 or 1:3
                    }
                    else if (wins == 3 && losses == 2)
                    {
                        return 2; // 2 points for a loss 2:3
                    }
                    else if (wins == 2 && losses == 3)
                    {
                        return 1; // 1 point for a win 3:2
                    }
                }

                // In case of other results, 0 points
                return 0;
            }

            // Counting points for matches as host
            foreach (var match in hostMatches)
            {
                int hostWins = 0;
                int guestWins = 0;

                // Checking set wins
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

            // Counting points for matches as guest
            foreach (var match in guestMatches)
            {
                int hostWins = 0;
                int guestWins = 0;

                // Checking set wins
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


    }
}