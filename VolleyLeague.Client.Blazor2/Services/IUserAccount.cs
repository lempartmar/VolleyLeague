using VolleyLeague.Client.Blazor2.Shared.Dtos;
using static VolleyLeague.Client.Blazor2.Shared.Dtos.ServiceResponses;

namespace VolleyLeague.Client.Blazor2.Services
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDto userDTO);
        Task<string> LoginAccount(LoginDto loginDTO);
    }
}