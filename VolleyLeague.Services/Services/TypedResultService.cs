using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Matches;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class TypedResultService : ITypedResultService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<TypedResult> _typedResultsRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        public TypedResultService(IBaseRepository<TypedResult> typedResult, IMapper mapper, IBaseRepository<Credentials> credentials) 
        {
            _typedResultsRepository = typedResult;
            _credentialsRepository = credentials;
            _mapper = mapper;   
        }

        public async Task CreateTypedResult(TypedResultDto typedResult)
        {
            // var newScore = _mapper.Map<TypedResult>(typedResult);

            var userId = _credentialsRepository.GetAll().Where(x => x.Email == typedResult.UserId).FirstOrDefault();
            TypedResult newScore = new TypedResult()
            {
                UserId = userId.UserId,
                Score1 = (byte)typedResult.Score1,
                Score2 = (byte)typedResult.Score2,
                MatchId = typedResult.MatchId,
            };

            await _typedResultsRepository.InsertAsync(newScore);
            await _typedResultsRepository.SaveChangesAsync();
        }
    
    public async Task<List<TypedUserDto>> GetTypedResults(int seasonId)
        {
                var typedResults = _typedResultsRepository.GetAll()
                    .Include(w => w.Match)
                    .Include(w => w.User)
                    .Where(x => x.Score1 + x.Score2 + x.Match.Team1Score + x.Match.Team2Score != 0 && x.Match.Round.SeasonId == seasonId);

            var perfectResults = typedResults
                .Where(w => w.Score1 == w.Match.Team1Score && w.Score2 == w.Match.Team2Score);

            var perfectTypers = perfectResults
                .GroupBy(w => w.User.Id)
                .Select(x => new {
                    id = x.Key,
                    points = x.Count() * 2,
                    countPerfectResults = x.Count(),
                    countCorrectResults = 0,
                    firstName = x.FirstOrDefault().User.FirstName,
                    lastName = x.FirstOrDefault().User.LastName
                });

            var correctResults = typedResults
                .Where(w => (w.Score1 > w.Score2 && w.Match.Team1Score > w.Match.Team2Score) || (w.Score1 < w.Score2 && w.Match.Team1Score < w.Match.Team2Score));

            var correctTypers = correctResults
                .GroupBy(w => w.User.Id)
                .Select(x => new {
                    id = x.Key,
                    points = x.Count(),
                    countPerfectResults = 0,
                    countCorrectResults = x.Count(),
                    firstName = x.FirstOrDefault().User.FirstName,
                    lastName = x.FirstOrDefault().User.LastName
                });

            var bestTypers = await correctTypers
                .Concat(perfectTypers)
                .GroupBy(x => x.id)
                .Select(x => new {
                    id = x.Key,
                    points = x.Sum(y => y.points),
                    countPerfectResults = x.Sum(y => y.countPerfectResults),
                    countCorrectResults = x.Sum(y => y.countCorrectResults),
                    firstName = x.FirstOrDefault().firstName,
                    lastName = x.FirstOrDefault().lastName
                })
                .OrderByDescending(x => x.points)
                .ToListAsync();

            List<TypedUserDto> typers = new List<TypedUserDto>();
            foreach (var typer in bestTypers)
            {
                typers.Add(new TypedUserDto()
                {
                    UserId = typer.id, 
                    TemporaryVote = typer.points.ToString(),
                    CorrectResultsCount = typer.countCorrectResults,
                    PerfectResultsCount = typer.countPerfectResults,
                    FirstName = typer.firstName,
                    LastName = typer.lastName
                });
            }

            return typers;
        }
    }
}
