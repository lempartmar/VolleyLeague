namespace VolleyLeague.Entities.Dtos.Users
{
    public class PasswordResetDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
