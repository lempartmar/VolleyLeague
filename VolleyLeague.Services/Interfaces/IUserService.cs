using VolleyLeague.Entities.Dtos.Users;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfile(int id);

        List<Role> Login(LoginDto loginDto, out Credentials? credentials);

        Task<UserProfileDto> GetUserProfileByEmail(string email);

        Task<bool> Register(RegisterDto registerDto);

        Task<bool> IsTeamCaptain(string playerEmail);
    }
}

