using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolleyLeague.Client.Blazor.Services;
using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;
using VolleyLeague.Services.Services;

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

            string issuer = _config.GetValue<string>("Jwt:Issuer");
            string audience = _config.GetValue<string>("Jwt:Audience");
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"));

            List<Claim> roles = credentials.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, credentials.Email),
                    new Claim(ClaimTypes.NameIdentifier, credentials.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(240),
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _userService.Register(registerDto);
            if (!result)
            {
                return BadRequest("Registration failed");
            }
            return Ok("Registration successful");
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
    }
}
