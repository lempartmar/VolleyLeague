using VolleyLeague.Client.Blazor.Shared.Dtos;
using static VolleyLeague.Client.Blazor.Shared.Dtos.ServiceResponses;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDto userDTO);
        Task<Shared.Dtos.LoginResponse> LoginAccount(LoginDto loginDTO);
    }
}