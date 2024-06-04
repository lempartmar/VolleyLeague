using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Entities.Dtos.Teams;
using VolleyLeague.Entities.Dtos.Users;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IUserService
    {
        Task Register(RegisterDto registerDto);
        //Task<string> Login(LoginDto loginDto);
        //Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto);
        Task<List<PositionDto>> GetPositions();
        Task<PlayerSummaryDto> GetUserSummary();
        Task<UserProfileDto> GetUserProfile(int userId);
        
        Task<bool> IsTeamCaptain();

        Task<UserProfileDto> GetCurrentUserProfile();
        Task UpdateUser(UpdateUserDto userProfileDto);

        Task<bool> RequestPasswordReset(PasswordResetRequestDto requestDto);

        Task<bool> ResetPassword(PasswordResetDto resetDto);

        Task<bool> IsTokenValid(string token);

        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

    }

    public class UserService : IUserService
    {
        private HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Register(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/register", registerDto);
            response.EnsureSuccessStatusCode();
        }

        //public async Task<string> Login(LoginDto loginDto)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/user/login", loginDto);
        //    response.EnsureSuccessStatusCode();

        //    var token = await response.Content.ReadAsStringAsync();
        //    return token;
        //}

        public async Task<List<PositionDto>> GetPositions()
        {
            var response = await _httpClient.GetAsync("api/position/getallpositions");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PositionDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<PlayerSummaryDto> GetUserSummary()
        {
            var response = await _httpClient.GetAsync("api/user/summary");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PlayerSummaryDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }

        public async Task<UserProfileDto> GetUserProfile(int userId)
        {
            var response = await _httpClient.GetAsync($"api/User/GetUserProfile/{userId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> RequestPasswordReset(PasswordResetRequestDto requestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/requestPasswordReset", requestDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPassword(PasswordResetDto resetDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/resetPassword", resetDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IsTokenValid(string token)
        {
            var response = await _httpClient.GetAsync($"api/user/isTokenValid?token={token}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/changePassword", changePasswordDto);
            return response.IsSuccessStatusCode;
        }


        //public async Task<string> Login(LoginDto loginDto)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync("api/User/User", loginDto);
        //        response.EnsureSuccessStatusCode();

        //        var content = await response.Content.ReadAsStringAsync();
        //        return content;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public async Task<bool> IsTeamCaptain()
        {
            var response = await _httpClient.GetAsync($"api/User/isteamcaptain");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<UserProfileDto> GetCurrentUserProfile()
        {
            var response = await _httpClient.GetAsync($"api/User/myprofile");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateUser(UpdateUserDto updateUserDto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/user/updateuserdata", updateUserDto);
            response.EnsureSuccessStatusCode();
        }

        //public async Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"api/user/{userId}/updatePassword", updatePasswordDto);
        //    response.EnsureSuccessStatusCode();
        //}
    }
}
