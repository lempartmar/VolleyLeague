using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace VolleyLeague.Client.Blazor.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string stringToken = await _localStorageService.GetItemAsStringAsync("token");
                if (string.IsNullOrWhiteSpace(stringToken))
                    return new AuthenticationState(_anonymous);

                var claims = AuthService.GetClaimsFromToken(stringToken);
                var claimsPrincipal = AuthService.SetClaimPrincipal(claims);

                // Add the token to the default request headers for the HttpClient
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", stringToken);

                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task UpdateAuthenticationState(string? token)
        {
            ClaimsPrincipal claimsPrincipal;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var userSession = AuthService.GetClaimsFromToken(token);
                claimsPrincipal = AuthService.SetClaimPrincipal(userSession);
                await _localStorageService.SetItemAsStringAsync("token", token);

                // Add the token to the default request headers for the HttpClient
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _localStorageService.RemoveItemAsync("token");
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public void NotifyUserAuthentication(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType"));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public static class AuthService
    {
        public static IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        public static ClaimsPrincipal SetClaimPrincipal(IEnumerable<Claim> claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));
        }
    }
}
