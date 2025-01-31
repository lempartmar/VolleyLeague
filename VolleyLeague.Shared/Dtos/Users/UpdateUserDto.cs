﻿using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Wzrost")]
        public int Height { get; set; }

        [Display(Name = "Waga")]
        public int Weight { get; set; }

        [Display(Name = "Numer koszulki")]
        public int JerseyNumber { get; set; }

        [Display(Name = "Zasięg ataku")]
        public int BlockRange { get; set; }

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
