using Blazored.LocalStorage;
using System.Net.Http;
using VolleyLeague.Client.Blazor2.Shared.Dtos;
using static VolleyLeague.Client.Blazor2.Shared.Dtos.ServiceResponses;
using System.Net.Http.Json;
using System.Text.Json;

namespace VolleyLeague.Client.Blazor2.Services
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

        public async Task<string> LoginAccount(LoginDto loginDTO)
        {
            //var response = await httpClient.PostAsJsonAsync("api/User/login", loginDTO);

            //if (!response.IsSuccessStatusCode)
            //    return new LoginResponse(false, null!, "Error occurred. Try again later...");

            //var apiResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(apiResponse); // Log the raw response

            //try
            //{
            //    return AuthService.DeserializeJsonString<LoginResponse>(apiResponse);
            //}
            //catch (JsonException ex)
            //{
            //    // Handle or log the exception as needed
            //    Console.WriteLine($"JSON Deserialization error: {ex.Message}");
            //    return new LoginResponse(false, null!, "Invalid JSON response.");
            //}

            var response = await httpClient.PostAsJsonAsync("api/User/login", loginDTO);
            var apiResponse = await response.Content.ReadAsStringAsync();
            ////Read Response
            //if (!response.IsSuccessStatusCode)
            //    return new LoginResponse(false, null!, "Error occured. Try again later...");

            //var apiResponse = await response.Content.ReadAsStringAsync();
            return apiResponse;
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
