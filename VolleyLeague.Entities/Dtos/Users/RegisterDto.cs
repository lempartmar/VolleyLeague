using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Entities.Dtos.Users
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        //public int PositionId { get; set; }

        //public bool Gender { get; set; }

        //[Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        //public string? PhoneNumber { get; set; }

        //[Range(1900, 2023, ErrorMessage = "Podaj poprawny rok urodzenia.")]
        //public int? BirthYear { get; set; }

        //[Range(50, 250, ErrorMessage = "Podaj prawidłowy wzrost w centymetrach")]
        //public int? Height { get; set; }

        //[Range(20, 500, ErrorMessage = "Podaj prawidłową wagę w kilogramach")]
        //public int? Weight { get; set; }

        //public byte[]? Photo { get; set; }

        //public string? Hobby { get; set; }

        //public int? JerseyNumber { get; set; }

        //public int? BlockRange { get; set; }

        //public int? AttackRange { get; set; }

        //public string? VolleyballIdol { get; set; }

        //public string? AdditionalEmail { get; set; }

        //public string? PersonalInfo { get; set; }

        //public string? City { get; set; }

    }
}