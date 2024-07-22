using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public class UserDefinedCodeService : IUserDefinedCodeService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<UserDefinedCode> _userDefinedCodeRepository;
        private readonly ILogService _logService;

        public UserDefinedCodeService(IBaseRepository<UserDefinedCode> userDefinedCodeRepository, ILogService logService, IMapper mapper)
        {
            _mapper = mapper;
            _userDefinedCodeRepository = userDefinedCodeRepository;
            _logService = logService;
        }

        public async Task<UserDefinedCode> GetCodeByKeyAsync(string key)
        {
            return await _userDefinedCodeRepository.GetAll().FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task UpdateCodeAsync(UserDefinedCode code)
        {
            var existingCode = await _userDefinedCodeRepository.GetAll().FirstOrDefaultAsync(c => c.Key == code.Key);
            if (existingCode != null)
            {
                existingCode.Value = code.Value;
                existingCode.ModifiedDate = DateTime.UtcNow;
                await _userDefinedCodeRepository.SaveChangesAsync();
            }
        }
    }
}

