using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        public UserService(IBaseRepository<User> userRepository, IBaseRepository<Credentials> credentialsRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _credentialsRepository = credentialsRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetUserProfile(int id)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == id);
            return _mapper.Map<UserProfileDto>(user);
        }


        // Service to login a user
        public List<Role> Login(LoginDto loginDto, out Credentials? credentials)
        {
            var response = new List<Role>();
            credentials = _credentialsRepository.GetAll().Include(c => c.Roles).FirstOrDefault(c => c.Email == loginDto.Login);


            if (credentials == null || !VerifyPassword(loginDto.Login, loginDto.Password, credentials.Password))
            {
                return null;
            }

            response = credentials.Roles.ToList();

            return response;
        }

        private bool VerifyPassword(string email, string password, string hashedPassword)
        {
            string hash = PepperPassowrd(password);
            var result = passwordHasher.VerifyHashedPassword(email, hashedPassword, hash);
            return result == PasswordVerificationResult.Success;
        }

        private string PepperPassowrd(string password)
        {
            string pepper = "qaSJKvYBm9$$;=EDOC-)EJ0m";
            return password + pepper;
        }
    }
}
