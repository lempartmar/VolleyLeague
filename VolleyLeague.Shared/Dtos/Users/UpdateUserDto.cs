using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [Display(Name = "Imię")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }

        [Range(1900, 2100, ErrorMessage = "Podaj prawidłowy rok urodzenia.")]
        [Display(Name = "Rok urodzenia")]
        public int BirthYear { get; set; }

        [Display(Name = "Miasto")]
        public string? City { get; set; }

        [Display(Name = "Dodatkowe informacje")]
        public string? PersonalInfo { get; set; }

        [Display(Name = "Zdjęcie")]
        public byte[]? Photo { get; set; }

        [Display(Name = "Płeć")]
        public bool Gender { get; set; }

        [Range(0, 300, ErrorMessage = "Podaj prawidłowy wzrost.")]
        [Display(Name = "Wzrost")]
        public int Height { get; set; }

        [Range(0, 300, ErrorMessage = "Podaj prawidłową wagę.")]
        [Display(Name = "Waga")]
        public int Weight { get; set; }

        [Range(0, 99, ErrorMessage = "Podaj prawidłowy numer koszulki.")]
        [Display(Name = "Numer koszulki")]
        public int JerseyNumber { get; set; }

        [Range(0, 400, ErrorMessage = "Podaj prawidłowy zasięg w ataku.")]
        [Display(Name = "Zasięg ataku")]
        public int BlockRange { get; set; }

        [Range(0, 400, ErrorMessage = "Podaj prawidłowy zasięg w bloku.")]
        [Display(Name = "Zasięg bloku")]
        public int AttackRange { get; set; }

        [Display(Name = "Idol siatkówki")]
        public string? VolleyballIdol { get; set; }

        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email.")]
        [Display(Name = "Dodatkowy email")]
        public string? AdditionalEmail { get; set; }

        [Display(Name = "Hobby")]
        public string? Hobby { get; set; }

        [Phone(ErrorMessage = "Podaj prawidłowy numer telefonu.")]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Pozycja jest wymagana.")]
        [Display(Name = "Pozycja")]
        public int PositionId { get; set; }
    }
}
