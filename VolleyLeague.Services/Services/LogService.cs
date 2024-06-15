using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public class LogService : ILogService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Log> _logRepository;
        private readonly IBaseRepository<PersonalLog> _personalLogRepository;

        public LogService(IMapper mapper, IBaseRepository<Log> logRepository, IBaseRepository<PersonalLog> personalLogRepository)
        {
            _mapper = mapper;
            _logRepository = logRepository;
            _personalLogRepository = personalLogRepository;
        }
    
        public async Task<List<LogDto>> GetLastTenLogs()
        {
            var result = await _logRepository.GetAll().OrderByDescending(x => x.Id).Take(10).ToListAsync();
            return _mapper.Map<List<LogDto>>(result);
        }

        public async Task AddLog(string description, string link, bool admin, User[] interestedUsers)
        {
            var newLog = new Log
            {
                Description = description,
                Link = link,
                Date = DateTime.Now,
                Admin = admin
            };

            await _logRepository.InsertAsync(newLog);
            await _logRepository.SaveChangesAsync();

            if (interestedUsers != null)
            {
                foreach (var user in interestedUsers)
                {
                    var personalLog = new PersonalLog
                    {
                        UserId = user.Id,
                        LogId = newLog.Id
                    };

                    await _personalLogRepository.InsertAsync(personalLog);
                }

                await _logRepository.SaveChangesAsync();
            }
        }
    }
}
