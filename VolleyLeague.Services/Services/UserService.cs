using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Credentials> _credentialsRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        private readonly IConfiguration _config;

        public UserService(IBaseRepository<User> userRepository,
                           IBaseRepository<Credentials> credentialsRepository,
                           IRoleRepository roleRepository,
                           IMapper mapper,
                           IConfiguration config)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _credentialsRepository = credentialsRepository;
            _mapper = mapper;
            _config = config;
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

        public async Task<PlayerSummaryDto> GetPlayerSummary(string email)
        {
            var response = new PlayerSummaryDto();

            var credentials = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(c => c.Email == email);

            if (credentials == null)
            {
                return null;
            }

            var player = new PlayerSummaryDto()
            {
                Id = credentials.User.Id,
                FirstName = credentials.User.FirstName,
                LastName = credentials.User.LastName,
                Photo = credentials.User.Photo,
            };

            return player;
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            var credentials = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(c => c.Email == userId);

            if (credentials == null)
            {
                return false;
            }

            var user = credentials.User;

            user.AttackRange = updateUserDto.AttackRange;
            user.BirthYear = updateUserDto.BirthYear;
            user.City = updateUserDto.City;
            user.BlockRange = updateUserDto.BlockRange;
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.Gender = updateUserDto.Gender;
            user.Height = (byte?)(updateUserDto.Height);
            user.Weight = (byte?)(updateUserDto.Weight);
            user.JerseyNumber = (byte?)(updateUserDto.JerseyNumber);
            user.VolleyballIdol = updateUserDto.VolleyballIdol;
            user.Hobby = updateUserDto.Hobby;
            user.Photo = updateUserDto.Photo;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.PositionId = updateUserDto.PositionId;
            user.PersonalInfo = updateUserDto.PersonalInfo;

            await _credentialsRepository.SaveChangesAsync();

            return true;
        }

        private bool VerifyPassword(string email, string password, string hashedPassword)
        {
            string hash = PepperPassword(password);
            var result = passwordHasher.VerifyHashedPassword(email, hashedPassword, hash);
            return result == PasswordVerificationResult.Success;
        }

        private string PepperPassword(string password)
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
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
            }
            else
            {
                user = ConvertToUser(registerDto);
                await _userRepository.InsertAsync(user);
            }

            var playerRoles = await _roleRepository.GetRoles();
            var playerRole = playerRoles.FirstOrDefault(r => r.Name == Roles.Player);

            var credentials = new Credentials
            {
                Email = registerDto.Email,
                Password = HashPassword(registerDto.Email, registerDto.Password),
                User = user,
                Roles = new List<Role> { playerRole }
            };

            await _credentialsRepository.InsertAsync(credentials);
            try
            {
                await _credentialsRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsTeamCaptain(string playerEmail)
        {
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

            var isCaptain = await _userRepository.GetAll()
                .AnyAsync(u => u.Id == user.Id && u.Team.CaptainId == user.Id);

            return isCaptain;
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var user = await _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefaultAsync(u => u.Credentials.Email == email);

            if (user == null)
            {
                return false;
            }

            var resetToken = GeneratePasswordResetToken(user.Credentials.Email);

            await SendPasswordResetEmail(user.Credentials.Email, resetToken);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    Console.WriteLine("Principal is null.");
                    return false;
                }

                var email = principal.FindFirst("email").ToString();

                string input = "email: liga.siatkowki12345@interia.pl";
                string prefix = "email: ";

                if (input.StartsWith(prefix))
                {
                    email = input.Substring(prefix.Length);
                    Console.WriteLine(email);
                }

                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Email claim is null or empty.");
                    return false;
                }

                var user = await _userRepository.GetAll()
                                                .Include(u => u.Credentials)
                                                .FirstOrDefaultAsync(u => u.Credentials.Email == email);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                user.Credentials.Password = HashPassword(user.Credentials.Email, newPassword);

                await _userRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting password: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> IsTokenValid(string token)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var email = principal.FindFirstValue(ClaimTypes.Email);

                var user = await _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefaultAsync(u => u.Credentials.Email == email);

                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var credentials = await _credentialsRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Email == email);

            if (credentials == null)
            {
                return false;
            }

            if (!VerifyPassword(email, currentPassword, credentials.Password))
            {
                return false;
            }

            credentials.Password = HashPassword(email, newPassword);
            await _credentialsRepository.SaveChangesAsync();
            return true;
        }

        private async Task SendPasswordResetEmail(string email, string resetToken)
        {
            var resetLink = $"https://localhost:7068/reset-password?token={resetToken}";
            var message = new MailMessage("noreply@yourwebsite.com", email)
            {
                Subject = "Resetowanie hasła",
                Body = $"Kliknij na poniższy link, aby zresetować hasło: <a href=\"{resetLink}\">Resetuj hasło</a>",
                IsBodyHtml = true,
            };

            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("ligasiatkowkidevelopment@gmail.com", "awonkpobrfhwvvck"),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (SmtpException smtpEx)
            {
                // Obsługa specyficznych wyjątków SMTP
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Obsługa innych wyjątków
                Console.WriteLine($"General Error: {ex.Message}");
            }
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
                PositionId = 6,
                PhotoWidth = null,
                PhotoHeight = null,
                Articles = new List<Article>(),
                Team = null,
                TeamPlayers = new List<TeamPlayer>(), // Initialize as needed
            };
        }

        private string HashPassword(string email, string password)
        {
            string hash = PepperPassword(password);
            hash = passwordHasher.HashPassword(email, hash);
            return hash;
        }



        private string GeneratePasswordResetToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, email)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false // Ignorujemy datę wygaśnięcia tokenu
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null)
                {
                    Console.WriteLine("Invalid JWT token.");
                    throw new SecurityTokenException("Invalid token");
                }

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Invalid JWT token algorithm.");
                    throw new SecurityTokenException("Invalid token");
                }

                var emailClaim = principal.FindFirst("email");
                if (emailClaim == null)
                {
                    Console.WriteLine("Email claim not found.");
                }
                else
                {
                    Console.WriteLine($"Email claim found: {emailClaim.Value}");
                }

                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation error: {ex.Message}");
                throw new SecurityTokenException("Invalid token");
            }
        }



    }
}
