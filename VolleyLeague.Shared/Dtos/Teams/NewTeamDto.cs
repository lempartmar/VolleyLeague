using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class NewTeamDto
    {
        [Display(Name = "Nazwa drużyny")]
        [Required(ErrorMessage = "Nazwa drużyny jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Email drużynowy")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email drużynowy jest wymagany")]
        public string Email { get; set; }

        [Display(Name = "Telefon kontaktowy")]
        [Required(ErrorMessage = "Telefon kontaktowy jest wymagany")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Display(Name = "Opis drużyny")]
        public string? TeamDescription { get; set; }

        [Display(Name = "Strona internetowa")]
        public string? Website { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Zdjęcie drużyny")]
        public byte[]? Image { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Logo drużyny")]
        public byte[]? Logo { get; set; }

        [Display(Name = "Zawodnicy")]
        public virtual List<TeamPlayerDto> Players { get; set; } = new List<TeamPlayerDto>();

        // Method to generate NewTeamDto from Team
        public static NewTeamDto GenerateNewTeamDto(Team team)
        {
            return new NewTeamDto
            {
                Name = team.Name,
                Email = team.Email,
                Phone = team.Phone,
                TeamDescription = team.TeamDescription,
                Website = team.Website,
                Image = team.Image,
                Logo = team.Logo
            };
        }
    }
}
