using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfile(int id);

        List<Role> Login(LoginDto loginDto, out Credentials? credentials);

        Task<(bool Success, string Message)> MigratePasswordsAsync();

        Task<(bool Success, string Message)> StartEmailVerification(RegisterEmailDto registerDto);

        Task<(bool Success, string Message)> CompleteEmailVerification(CompleteEmailRegistrationDto completeEmailRegistrationDto, string userName);

        Task<UserProfileDto> GetUserProfileByEmail(string email);

        Task<bool> GetHasUserEmail(string identity);

        Task<(bool Success, string Message)> CompleteRegistration(CompleteRegistrationDto completeRegistrationDto);

        Task<(bool Success, string Message)> StartRegistration(RegisterDto registerDto);

        Task<(bool Success, string Message)> Register(RegisterDto registerDto);

        Task<bool> IsTeamCaptain(string playerEmail);

        Task<PlayerSummaryDto> GetPlayerSummary(string email);

        Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);

        Task<bool> ResetPasswordAsync(string token, string newPassword);

        Task<bool> RequestPasswordResetAsync(string email);

        Task<bool> IsTokenValid(string token);

        Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    }
}

