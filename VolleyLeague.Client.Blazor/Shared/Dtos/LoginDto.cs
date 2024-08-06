using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Client.Blazor.Shared.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Adres e-mail lub nazwa użytkownika jest wymagana.")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
