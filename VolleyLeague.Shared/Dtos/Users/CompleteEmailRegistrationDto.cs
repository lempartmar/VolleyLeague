using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class CompleteEmailRegistrationDto
    {
        public string? Email { get; set; }
        public string? VerificationCode { get; set; }
    }
}
