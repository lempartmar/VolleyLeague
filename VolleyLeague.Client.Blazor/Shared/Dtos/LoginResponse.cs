namespace VolleyLeague.Client.Blazor.Shared.Dtos
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }

}
