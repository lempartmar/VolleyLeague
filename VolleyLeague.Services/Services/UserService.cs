using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
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
        private readonly IBaseRepository<UserRegistrationVerificationCode> _verificationCodeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        private readonly IConfiguration _config;

        public UserService(IBaseRepository<User> userRepository,
                           IBaseRepository<Credentials> credentialsRepository,
                           IBaseRepository<UserRegistrationVerificationCode> userRegistrationVerificationCodeRepository,
                            IRoleRepository roleRepository,
                           IEmailService emailService,
                           IWebHostEnvironment env,
                           IFileService fileService,
                           IMapper mapper,
                           IConfiguration config)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _fileService = fileService;
            _env = env;
            _verificationCodeRepository = userRegistrationVerificationCodeRepository;
            _credentialsRepository = credentialsRepository;
            _emailService = emailService;
            _mapper = mapper;
            _config = config;
        }

        public async Task<UserProfileDto> GetUserProfile(int id)
        {
            var user = await _userRepository
                .GetAll()
                .Include(u => u.Position)
                .Include(u => u.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                .FirstOrDefaultAsync(u => u.Id == id);
            return _mapper.Map<UserProfileDto>(user);
        }

        public List<Role> Login(LoginDto loginDto, out Credentials? credentials)
        {
            var response = new List<Role>();
            var rehashNeeded = false;

            credentials = _credentialsRepository.GetAll()
                .Include(c => c.Roles)
                .Include(c => c.User)
                .FirstOrDefault(c => c.Email == loginDto.Identifier || c.UserName == loginDto.Identifier);


            if (credentials.Email != null && credentials.Email != loginDto.Identifier)
            {
                return null;
            }

            if (credentials == null || !VerifyPassword(credentials, loginDto.Password, out rehashNeeded))
            {
                return null;
            }

            if (rehashNeeded)
            {
                credentials.Password = passwordHasher.HashPassword(null, PepperPassword(loginDto.Password));
                _credentialsRepository.Update(credentials);
                _credentialsRepository.SaveChangesAsync();
            }

            if (credentials.Email != null)
            {
                AddAdditionalEmailIfMissing(credentials);
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

        public async Task<(bool Success, string Message)> MigratePasswordsAsync()
        {
            try
            {
                var credentialsList = await _credentialsRepository.GetAll()
                    .Where(c => !string.IsNullOrEmpty(c.OldPassword))
                    .ToListAsync();

                foreach (var credentials in credentialsList)
                {
                    var hashedPassword = passwordHasher.HashPassword(null, PepperPassword(credentials.OldPassword));
                    credentials.Password = hashedPassword;

                    credentials.OldPassword = null;
                }

                await _credentialsRepository.SaveChangesAsync();

                return (true, "Hasła zostały przeniesione, a kolumna OldPassword została pomyślnie wyczyszczona.");
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas migracji haseł. {ex}");
            }
        }

        public async Task<bool> GetHasUserEmail(string identity)
        {

            var credentials = await _credentialsRepository.GetAll().Include(c => c.User).FirstOrDefaultAsync(c => c.Email == identity || c.UserName == identity || c.LoweredUserName == identity);

            if (credentials?.Email != null)
            {
                return true;
            }

            return false;
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
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, hash);
            return result == PasswordVerificationResult.Success;
        }

        private bool VerifyPassword(Credentials credentials, string providedPassword, out bool rehashNeeded)
        {
            string pepperedPassword = PepperPassword(providedPassword);
            // Weryfikacja hasła
            var result = passwordHasher.VerifyHashedPassword(null, credentials.Password, pepperedPassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }

        private string PepperPassword(string password)
        {
            string pepper = "Z@4F!jW9pL+q82%m*C7e^RsD3T6#UjY8>g!8hS$QmXz2&7PqRfVwB@Nc^Yh5t7Bv";
            return password + pepper;
        }

        public async Task<(bool Success, string Message)> Register(RegisterDto registerDto)
        {
            var existingUser = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.Credentials != null && u.Credentials.Email == registerDto.Email);

            if (existingUser != null)
            {
                return (false, "Użytkownik z takim adresem e-mail już istnieje.");
            }

            var user = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.AdditionalEmail == registerDto.Email);

            if (user != null && user.Credentials == null)
            {
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
                user.AdditionalEmail = registerDto.Email;
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
                Password = HashPassword(registerDto.Password),
                User = user,
                Roles = new List<Role> { playerRole }
            };

            await _credentialsRepository.InsertAsync(credentials);
            try
            {
                var logoPath = _fileService.GetLogoPath();
                var createTeamImage = _fileService.GetPathForEmail("create_team");
                var typermaniaImage = _fileService.GetPathForEmail("Typermania");

                var message = new MailMessage("noreply@yourwebsite.com", registerDto.Email)
                {
                    Subject = "Witamy w ligasiatkowki.pl",
                    Body = @"<html>
                        <body style='font-family: Roboto, serif; font-size: 13px; color: #333; text-align: center;'>
                            <img src='cid:LigaSiatkowkiLogo' alt='Liga Siatkówki' style='width:100%; max-width:500px'/>
                            <p>Drogi użytkowniku,</p>
                            <p>Zostałeś pomyślnie zarejestrowany w portalu ligasiatkowki.pl. Portal ligasiatkowki.pl umożliwia tworzenie drużyn oraz wzięcie udziału w naszej ekscytującej typermanii!</p>
                            <h2>Twórz Drużyny</h2>
                            <p>Dołącz do drużyny lub stwórz własną, aby rywalizować z innymi w naszej lidze siatkówki.</p>
                            <img src='cid:CreateTeamImage' alt='Twórz Drużyny' style='width:100%; max-width:600px;' />
                            <h2>Weź Udział w Typermanii</h2>
                            <p>Typuj wyniki meczów i zdobywaj punkty, rywalizując z innymi użytkownikami w typermanii.</p>
                            <img src='cid:TypermaniaImage' alt='Typermania' style='width:100%; max-width:600px;' />
                            <p>Serdecznie zapraszamy do aktywnego udziału!</p>
                        </body>
                     </html>",
                    IsBodyHtml = true,
                };

                if (File.Exists(logoPath))
                {
                    Attachment logoAttachment = new Attachment(logoPath, MediaTypeNames.Image.Jpeg);
                    logoAttachment.ContentId = "LigaSiatkowkiLogo";
                    logoAttachment.ContentDisposition.Inline = true;
                    logoAttachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    message.Attachments.Add(logoAttachment);
                }

                if (File.Exists(createTeamImage))
                {
                    Attachment createTeamAttachment = new Attachment(createTeamImage, MediaTypeNames.Image.Jpeg);
                    createTeamAttachment.ContentId = "CreateTeamImage";
                    createTeamAttachment.ContentDisposition.Inline = true;
                    createTeamAttachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    message.Attachments.Add(createTeamAttachment);
                }

                if (File.Exists(typermaniaImage))
                {
                    Attachment typermaniaAttachment = new Attachment(typermaniaImage, MediaTypeNames.Image.Jpeg);
                    typermaniaAttachment.ContentId = "TypermaniaImage";
                    typermaniaAttachment.ContentDisposition.Inline = true;
                    typermaniaAttachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    message.Attachments.Add(typermaniaAttachment);
                }

                await _credentialsRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, "Rejestracja nie powiodła się z powodu błędu serwera.");
            }
            return (true, "Rejestracja przebiegła pomyślnie");
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
                    return false;
                }

                var emailClaim = principal.FindFirst("email");
                if (emailClaim == null)
                {
                    return false;
                }

                var email = emailClaim.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                var user = await _userRepository.GetAll()
                                                .Include(u => u.Credentials)
                                                .FirstOrDefaultAsync(u => u.Credentials.Email == email);
                if (user == null)
                {
                    return false;
                }

                user.Credentials.Password = HashPassword(newPassword);

                await _userRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd resetowania hasła: {ex.Message}");
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

        public async Task<(bool Success, string Message)> StartRegistration(RegisterDto registerDto)
        {
            var existingUser = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.Credentials != null && u.Credentials.Email == registerDto.Email);

            if (existingUser != null)
            {
                return (false, "Użytkownik z takim adresem e-mail już istnieje.");
            }

            var verificationCode = GenerateVerificationCode();
            var expirationTime = DateTime.UtcNow.AddMinutes(30);

            var verificationEntity = new UserRegistrationVerificationCode
            {
                Email = registerDto.Email,
                Code = verificationCode,
                ExpirationTime = expirationTime
            };

            await _verificationCodeRepository.InsertAsync(verificationEntity);
            await _verificationCodeRepository.SaveChangesAsync();

            await SendVerificationEmail(registerDto.Email, verificationCode);

            return (true, "Kod weryfikacyjny wysłany na e-mail.");
        }

        public async Task<(bool Success, string Message)> StartEmailVerification(RegisterEmailDto registerDto)
        {
            var existingUser = _userRepository.GetAll().Include(u => u.Credentials).FirstOrDefault(u => u.Credentials != null && u.Credentials.Email == registerDto.Email);

            if (existingUser != null)
            {
                return (false, "Użytkownik z takim adresem e-mail już istnieje.");
            }

            var verificationCode = GenerateVerificationCode();
            var expirationTime = DateTime.UtcNow.AddMinutes(30);

            var verificationEntity = new UserRegistrationVerificationCode
            {
                Email = registerDto.Email,
                Code = verificationCode,
                ExpirationTime = expirationTime
            };

            await _verificationCodeRepository.InsertAsync(verificationEntity);
            await _verificationCodeRepository.SaveChangesAsync();

            await SendVerificationEmail(registerDto.Email, verificationCode);

            return (true, "Kod weryfikacyjny wysłany na e-mail.");
        }

        public async Task<(bool Success, string Message)> CompleteEmailVerification(CompleteEmailRegistrationDto completeEmailRegistrationDto, string userName)
        {
            using var transaction = await _userRepository.BeginTransactionAsync();

            try
            {
                using (var verificationContext = _verificationCodeRepository.CreateContext())
                {
                    var verificationEntity = await verificationContext.Set<UserRegistrationVerificationCode>()
                        .FirstOrDefaultAsync(vc => vc.Email == completeEmailRegistrationDto.Email && vc.Code == completeEmailRegistrationDto.VerificationCode);

                    if (verificationEntity == null || verificationEntity.ExpirationTime < DateTime.UtcNow)
                    {
                        return (false, "Nieprawidłowy lub wygasły kod weryfikacyjny.");
                    }

                    var credentials = await _credentialsRepository.GetAll().Where(x => x.UserName == userName || x.Email == userName).FirstOrDefaultAsync();
                    credentials.Email = completeEmailRegistrationDto.Email;

                    var user = await _userRepository.GetAll().Where(x => x.Id == credentials.UserId).FirstOrDefaultAsync();
                    user.AdditionalEmail = completeEmailRegistrationDto.Email;

                    await _userRepository.SaveChangesAsync();
                    await _credentialsRepository.SaveChangesAsync();

                    await transaction.CommitAsync();
                }

                return (true, "Zakończono z sukcesem weryfikację adresu e-mail");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Rejestracja e-maila zakończona niepowodzeniem: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> CompleteRegistration(CompleteRegistrationDto completeRegistrationDto)
        {
            using var transaction = await _userRepository.BeginTransactionAsync();

            try
            {
                using (var verificationContext = _verificationCodeRepository.CreateContext())
                {
                    var verificationEntity = await verificationContext.Set<UserRegistrationVerificationCode>()
                        .FirstOrDefaultAsync(vc => vc.Email == completeRegistrationDto.Email && vc.Code == completeRegistrationDto.VerificationCode);

                    if (verificationEntity == null || verificationEntity.ExpirationTime < DateTime.UtcNow)
                    {
                        return (false, "Nieprawidłowy lub wygasły kod weryfikacyjny.");
                    }
                }

                var user = await _userRepository.GetAll()
                    .Include(u => u.Credentials)
                    .FirstOrDefaultAsync(u => u.AdditionalEmail == completeRegistrationDto.Email);

                if (user != null && user.Credentials == null)
                {
                    user.FirstName = completeRegistrationDto.FirstName;
                    user.LastName = completeRegistrationDto.LastName;
                    user.AdditionalEmail = completeRegistrationDto.Email;
                }
                else
                {
                    user = new User
                    {
                        FirstName = completeRegistrationDto.FirstName,
                        LastName = completeRegistrationDto.LastName,
                        AdditionalEmail = completeRegistrationDto.Email,
                        Gender = completeRegistrationDto.Gender,
                        PositionId = 6,
                    };
                    await _userRepository.InsertAsync(user);
                }

                var playerRoles = await _roleRepository.GetRoles();
                var playerRole = playerRoles.FirstOrDefault(r => r.Name == Roles.Player);

                var credentials = new Credentials
                {
                    Email = completeRegistrationDto.Email,
                    Password = HashPassword(completeRegistrationDto.Password),
                    User = user,
                    Roles = new List<Role> { playerRole }
                };

                await _credentialsRepository.InsertAsync(credentials);

                await _credentialsRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();

                using (var verificationContext = _verificationCodeRepository.CreateContext())
                {
                    var verificationEntity = await verificationContext.Set<UserRegistrationVerificationCode>()
                        .FirstOrDefaultAsync(vc => vc.Email == completeRegistrationDto.Email && vc.Code == completeRegistrationDto.VerificationCode);

                    if (verificationEntity != null)
                    {
                        verificationContext.Set<UserRegistrationVerificationCode>().Remove(verificationEntity);
                        await verificationContext.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                return (true, "Rejestracja przebiegła pomyślnie");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Rejestracja nie powiodła się z powodu błędu serwera: {ex.Message}");
            }
        }


        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendVerificationEmail(string email, string verificationCode)
        {
            string resourcePath = "VolleyLeague.Services.EmailTemplates.VerificationEmailTemplate.html";

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Nie znaleziono pliku szablonu wiadomości e-mail: {resourcePath}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string emailTemplate = await reader.ReadToEndAsync();
                    string emailBody = emailTemplate.Replace("623123", verificationCode);

                    var message = new MailMessage("noreply@yourwebsite.com", email)
                    {
                        Subject = "Verification Code",
                        Body = emailBody,
                        IsBodyHtml = true,
                    };

                    await _emailService.Send(email, message);
                }
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

            credentials.Password = HashPassword(newPassword);
            await _credentialsRepository.SaveChangesAsync();
            return true;
        }

        private async Task SendPasswordResetEmail(string email, string resetToken)
        {
            var resetLink = $"https://volleyleagueapi20240804.azurewebsites.net/reset-password?token={resetToken}";
            string resourcePath = "VolleyLeague.Services.EmailTemplates.PasswordResetTemplate.html";

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Nie znaleziono pliku szablonu wiadomości e-mail: {resourcePath}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string emailTemplate = await reader.ReadToEndAsync();
                    string emailBody = emailTemplate.Replace("https://tabular.email", resetLink);

                    var message = new MailMessage("noreply@yourwebsite.com", email)
                    {
                        Subject = "Resetowanie hasła",
                        Body = emailBody,
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
                        Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General Error: {ex.Message}");
                    }
                }
            }
        }

        private User ConvertToUser(RegisterDto registerDto)
        {
            return new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                AccountId = null,
                AdditionalEmail = registerDto.Email,
                Gender = registerDto.Gender == 1,
                PositionId = 6,
                PhotoWidth = null,
                PhotoHeight = null,
                Articles = new List<Article>(),
                Team = null,
                TeamPlayers = new List<TeamPlayer>(),
            };
        }

        private string HashPassword(string password)
        {
            string hash = PepperPassword(password);
            hash = passwordHasher.HashPassword(null, hash);
            return hash;
        }

        private void AddAdditionalEmailIfMissing(Credentials credentials)
        {
            if (credentials.User != null && string.IsNullOrEmpty(credentials.User.AdditionalEmail))
            {
                credentials.User.AdditionalEmail = credentials.Email;
                _userRepository.Update(credentials.User);
                _userRepository.SaveChangesAsync();
            }
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
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var emailClaim = principal.FindFirst("email");

                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
    }
}
