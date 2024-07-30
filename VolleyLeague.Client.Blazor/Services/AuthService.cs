using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using VolleyLeague.Client.Blazor.Shared.Dtos;

namespace VolleyLeague.Client.Blazor.Services
{
    public static class AuthService
    {
        public static ClaimsPrincipal SetClaimPrincipal(UserSession model)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, model.Id!),
        new Claim(ClaimTypes.Name, model.Name!)
    };

            foreach (var role in model.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "JwtAuth");

            return new ClaimsPrincipal(identity);
        }
        public static UserSession GetClaimsFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var claims = token.Claims;

            string id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;
            string name = claims.First(c => c.Type == ClaimTypes.Name).Value!;

            var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return new UserSession(id, name, roles);
        }

        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }
        public static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());

        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;
        
        public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");
    }
}
