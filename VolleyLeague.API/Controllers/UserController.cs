using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly Services.Services.IUserService _userService;
        private readonly IConfiguration _config;

        public UserController(ILogger<UserController> logger, Services.Services.IUserService userService, IConfiguration config)
        {
            _logger = logger;
            _userService = userService;
            _config = config;
        }

        [HttpGet("GetUserProfile/{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var result = await _userService.GetUserProfile(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var serviceResult = _userService.Login(loginDto, out Credentials? credentials);

            if (serviceResult == null || credentials == null)
            {
                return Unauthorized();
            }

            // U¿ycie Email lub UserName jako alternatywy, jeœli Email jest null
            var identifier = credentials.Email ?? credentials.UserName;
            if (string.IsNullOrEmpty(identifier))
            {
                return BadRequest("Email or Username cannot be null or empty.");
            }

            string issuer = _config.GetValue<string>("Jwt:Issuer");
            string audience = _config.GetValue<string>("Jwt:Audience");
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"));

            List<Claim> roles = credentials.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, identifier),
            new Claim(ClaimTypes.NameIdentifier, identifier),
        }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            tokenDescriptor.Subject.AddClaims(roles);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.OutboundClaimTypeMap.Clear();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return Ok(stringToken);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _userService.Register(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Registration successful");
        }

        [AllowAnonymous]
        [HttpPost("startregistration")]
        public async Task<IActionResult> StartRegistration([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.StartRegistration(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Verification code sent to email.");
        }

        [Authorize]
        [HttpPost("startemailverification")]
        public async Task<IActionResult> StartEmailVerification([FromBody] RegisterEmailDto registerDto)
        {
            var result = await _userService.StartEmailVerification(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Verification code sent to email.");
        }

        [AllowAnonymous]
        [HttpPost("completeregistration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationDto completeRegistrationDto)
        {
            var result = await _userService.CompleteRegistration(completeRegistrationDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Registration successful.");
        }

        [Authorize]
        [HttpPost("completeemailverification")]
        public async Task<IActionResult> CompleteEmailVerification([FromBody] CompleteEmailRegistrationDto completeRegistrationDto)
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return NotFound();
            }

            var result = await _userService.CompleteEmailVerification(completeRegistrationDto, userName);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Registration successful.");
        }


        [Authorize]
        [HttpGet("summary")]
        public async Task<IActionResult> GetUserSummary()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var result = await _userService.GetPlayerSummary(id);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetHasUserEmail")]
        public async Task<IActionResult> GetHasUserEmail()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var result = await _userService.GetHasUserEmail(id);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("myprofile")]
        public async Task<IActionResult> GetMyProfile()
        {
            string? email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(email))
            {
                return NotFound();
            }
            var result = await _userService.GetUserProfileByEmail(email);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPut("updateuserdata")]
        public async Task<IActionResult> UpdateUserData(UpdateUserDto updateUserDto)
        {
            string? id = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var result = await _userService.UpdateUserAsync(id, updateUserDto);

            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("isTeamCaptain")]
        public async Task<IActionResult> IsTeamCaptain()
        {
            string? email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(email))
            {
                return NotFound();
            }
            var result = await _userService.IsTeamCaptain(email);
            return Ok(result);
        }

        [HttpPost("requestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequestDto requestDto)
        {
            var result = await _userService.RequestPasswordResetAsync(requestDto.Email);
            if (!result)
            {
                return BadRequest("Invalid email address");
            }
            return Ok("Password reset email sent");
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetDto resetDto)
        {
            var result = await _userService.ResetPasswordAsync(resetDto.Token, resetDto.NewPassword);
            if (!result)
            {
                return BadRequest("Invalid token or token expired");
            }
            return Ok("Password has been reset successfully");
        }

        [HttpGet("isTokenValid")]
        public async Task<IActionResult> IsTokenValid([FromQuery] string token)
        {
            var result = await _userService.IsTokenValid(token);
            if (!result)
            {
                return BadRequest("Invalid token or token expired");
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            string? email = User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangePasswordAsync(email, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (result)
            {
                return Ok(new { message = "Password changed successfully" });
            }

            return BadRequest(new { message = "Failed to change password" });
        }
    }
}
