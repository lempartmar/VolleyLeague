using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;

namespace VolleyLeague.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        public UserService(IBaseRepository<User> userRepository, 
            IBaseRepository<Credentials> credentialsRepository, 
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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
            credentials = _credentialsRepository.GetAll().Include(c => c.Roles).FirstOrDefault(c => c.Email == loginDto.Email);


            if (credentials == null || !VerifyPassword(loginDto.Email, loginDto.Password, credentials.Password))
            {
                return null;
            }

            response = credentials.Roles.ToList();

            return response;
        }

        public async Task<UserProfileDto> GetUserProfileByEmail(string email)
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.Credentials)
                .FirstOrDefaultAsync(u => u.Credentials != null && u.Credentials.Email == email);
            return _mapper.Map<UserProfileDto>(user);
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

        public async Task<bool> Register(RegisterDto registerDto)
        {
            var existingUser = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.Credentials != null && u.Credentials.Email == registerDto.Email);

            if (existingUser != null)
            {
                return false;
            }

            var user = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.AdditionalEmail == registerDto.Email);

            if (user != null && user.Credentials == null)
            {
                // someone has added this email to a team
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
                //user.Phone = registerDto.PhoneNumber;
                //user.PositionId = registerDto.PositionId;
                //user.BirthYear = registerDto.BirthYear;
                //user.Height = (byte?)registerDto.Height;
                //user.Weight = (byte?)registerDto.Weight;
                //user.JerseyNumber = (byte?)registerDto.JerseyNumber;
                //user.AttackRange = (byte?)registerDto.AttackRange;
                //user.BlockRange = (byte?)registerDto.BlockRange;
                //user.VolleyballIdol = registerDto.VolleyballIdol;
                //user.AdditionalEmail = registerDto.AdditionalEmail;
                //user.PersonalInfo = registerDto.PersonalInfo;
                //user.City = registerDto.City;
                //user.Hobby = registerDto.Hobby;
            }
            else
            {
                user = ConvertToUser(registerDto);
                await _userRepository.InsertAsync(user);
            }


            var playerRoles = await _roleRepository.GetRoles();
            var playerRole = playerRoles.Where(r => r.Name == Roles.Player).FirstOrDefault();

            var credentials = new Credentials
            {
                Email = registerDto.Email,
                Password = HashPassword(registerDto.Email, registerDto.Password), // Hash the password
                User = user,
                Roles = new List<Role> { playerRole }

            };

            await _credentialsRepository.InsertAsync(credentials);
            try
            {
                await _credentialsRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
                //response.Message = "Błąd zapisu danych. Spróbuj ponownie później";
            }
            return true;
        }

        public async Task<bool> IsTeamCaptain(string playerEmail)
        {
            // Find the user by email
            var user = await _credentialsRepository.GetAll()
                          .Include(c => c.User)
                          .ThenInclude(u => u.Team)
                          .Where(c => c.Email == playerEmail)
                          .Select(c => c.User)
                          .FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            // Check if the user is a captain of any team
            var isCaptain = await _userRepository.GetAll()
                              .AnyAsync(u => u.Id == user.Id && u.Team.CaptainId == user.Id);

            return isCaptain;
        }


        private User ConvertToUser(RegisterDto registerDto)
        {
            return new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                AccountId = null,
                //BirthYear = registerDto.BirthYear,
                //City = registerDto.City,
                //PersonalInfo = registerDto.PersonalInfo,
                //Photo = null,
                //Gender = registerDto.Gender,
                //Height = (byte?)registerDto.Height,
                //Weight = (byte?)registerDto.Weight,
                //JerseyNumber = (byte?)registerDto.JerseyNumber,
                //BlockRange = registerDto.BlockRange,
                //AttackRange = registerDto.AttackRange,
                //VolleyballIdol = registerDto.VolleyballIdol,
                //AdditionalEmail = registerDto.AdditionalEmail,
                //Hobby = registerDto.Hobby,
                //Phone = null,
                //PositionId = registerDto.PositionId > 0 ? registerDto.PositionId : 1,
                PhotoWidth = null,
                PhotoHeight = null,
                Articles = new List<Article>(),
                Team = null,
                TeamPlayers = new List<TeamPlayer>(), // Initialize as needed
            };
        }

        private string HashPassword(string email, string password)
        {
            string hash = PepperPassowrd(password);

            hash = passwordHasher.HashPassword(email, hash);
            return hash;

        }
    }
}
