using Blazored.LocalStorage;
using System.Net.Http.Json;
using VolleyLeague.Client.Blazor.Shared.Dtos;
using static VolleyLeague.Client.Blazor.Shared.Dtos.ServiceResponses;

namespace VolleyLeague.Client.Blazor.Services
{
    public class AccountService : IUserAccount
    {
        public AccountService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }
        private const string BaseUrl = "api/Account";
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;

        public async Task<GeneralResponse> CreateAccount(UserDto userDTO)
        {
            var response = await httpClient
                 .PostAsync($"{BaseUrl}/register",
                 AuthService.GenerateStringContent(
                 AuthService.SerializeObj(userDTO)));

            //Read Response
            if (!response.IsSuccessStatusCode)
                return new GeneralResponse(false, "Error occured. Try again later...");

            var apiResponse = await response.Content.ReadAsStringAsync();
            return AuthService.DeserializeJsonString<GeneralResponse>(apiResponse);
        }

        public async Task<Shared.Dtos.LoginResponse> LoginAccount(LoginDto loginDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/User/login", loginDTO);

            if (!response.IsSuccessStatusCode)
            {
                return new Shared.Dtos.LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Błędny email lub hasło. Spróbuj ponownie."
                };
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            return new Shared.Dtos.LoginResponse
            {
                Success = true,
                Token = apiResponse
            };
        }


        //public async Task<LoginResponse> LoginAccount(LoginDto loginDTO)
        //    {
        //    var response = await httpClient.PostAsJsonAsync("api/User/login", loginDTO);
        //    //var response = await httpClient
        //    //       .PostAsync("https://localhost:7068/api/User/login",
        //    //       AuthService.GenerateStringContent(
        //    //       AuthService.SerializeObj(loginDTO)));

        //        //Read Response
        //        if (!response.IsSuccessStatusCode)
        //            return new LoginResponse(false, null!, "Error occured. Try again later...");
        //        Console.WriteLine(response);
        //        var apiResponse = await response.Content.ReadAsStringAsync();
        //        return AuthService.DeserializeJsonString<LoginResponse>(apiResponse);

        //    }
    }
}
