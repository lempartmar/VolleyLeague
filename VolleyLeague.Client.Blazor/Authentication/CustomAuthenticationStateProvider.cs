using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VolleyLeague.Client.Blazor.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(HttpClient httpClient, ISessionStorageService sessionStorageService)
        {
            _httpClient = httpClient;
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string stringToken = await _sessionStorageService.GetItemAsStringAsync("authToken");
                if (string.IsNullOrWhiteSpace(stringToken))
                    return new AuthenticationState(_anonymous);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(stringToken);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    await NotifyUserLogout();
                    return new AuthenticationState(_anonymous);
                }

                var claims = jwtToken.Claims;
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stringToken);

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
                var userSession = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userSession, "jwtAuthType"));
                await _sessionStorageService.SetItemAsStringAsync("authToken", token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _sessionStorageService.RemoveItemAsync("authToken");
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

        public async Task NotifyUserLogout()
        {
            await _sessionStorageService.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
