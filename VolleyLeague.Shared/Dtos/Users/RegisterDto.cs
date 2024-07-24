using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class RegisterDto : IValidatableObject
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Płeć jest wymagana.")]
        [Display(Name = "Płeć")]
        public int Gender { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Password.Length < 8)
            {
                validationResults.Add(new ValidationResult("Hasło musi zawierać co najmniej 8 znaków.", new[] { nameof(Password) }));
            }
            if (!Regex.IsMatch(Password, @"[A-Z]"))
            {
                validationResults.Add(new ValidationResult("Hasło musi zawierać co najmniej jedną wielką literę.", new[] { nameof(Password) }));
            }
            if (!Regex.IsMatch(Password, @"[a-z]"))
            {
                validationResults.Add(new ValidationResult("Hasło musi zawierać co najmniej jedną małą literę.", new[] { nameof(Password) }));
            }
            if (!Regex.IsMatch(Password, @"[0-9]"))
            {
                validationResults.Add(new ValidationResult("Hasło musi zawierać co najmniej jedną cyfrę.", new[] { nameof(Password) }));
            }
            if (!Regex.IsMatch(Password, @"[\p{P}\p{S}]"))
            {
                validationResults.Add(new ValidationResult("Hasło musi zawierać co najmniej jeden znak interpunkcyjny.", new[] { nameof(Password) }));
            }

            return validationResults;
        }
    }
}
