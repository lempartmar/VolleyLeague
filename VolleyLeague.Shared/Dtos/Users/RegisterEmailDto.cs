using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class RegisterEmailDto
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }
   }
}
