using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public UserController(ILogger<UserController> logger, IUserService userService, IConfiguration config)
        {
            _logger = logger;
            _userService = userService;
            _config = config;
        }

        [HttpGet("GetUserProfile/{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var result = await _userService.GetUserProfile(id);
            return Ok(result);
        }

        // Log in
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {

            var serviceResult = _userService.Login(loginDto, out Credentials? credentials);

            var response = new String("");

            if (serviceResult == null || credentials == null)
            {
                return null;
            }

            string issuer = _config.GetValue<string>("Jwt:Issuer");
            string audience = _config.GetValue<string>("Jwt:Audience");
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"));

            List<Claim> roles = new List<Claim>();

            foreach (var role in credentials.Roles)
            {
                roles.Add(new Claim(ClaimTypes.Role, role.Name));
            }

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

            response = stringToken;

            return Ok(response);

        }
    }
}