using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class AdminDefinedCodeService : IAdminDefinedCodeService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<AdminDefinedCode> _adminDefinedCodeRepository;
        private readonly ILogService _logService;

        public AdminDefinedCodeService(IBaseRepository<AdminDefinedCode> adminDefinedCodeRepository, ILogService logService, IMapper mapper)
        {
            _mapper = mapper;
            _adminDefinedCodeRepository = adminDefinedCodeRepository;
            _logService = logService;
        }

        public async Task<AdminDefinedCode> GetCodeByKeyAsync(string key)
        {
            return await _adminDefinedCodeRepository.GetAll().FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task UpdateCodeAsync(AdminDefinedCode code)
        {
            var existingCode = await _adminDefinedCodeRepository.GetAll().FirstOrDefaultAsync(c => c.Key == code.Key);
            if (existingCode != null)
            {
                existingCode.Value = code.Value;
                existingCode.ModifiedDate = DateTime.UtcNow;
                await _adminDefinedCodeRepository.SaveChangesAsync();
            }
        }
    }
}

