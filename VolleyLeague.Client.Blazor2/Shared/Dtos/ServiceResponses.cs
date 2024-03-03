namespace VolleyLeague.Client.Blazor2.Shared.Dtos
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LoginResponse(bool Flag, string Token, string Message);

    }
}
