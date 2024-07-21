using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class CompleteRegistrationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? VerificationCode { get; set; }
    }
}
