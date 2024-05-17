using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Entities.Dtos.Teams
{
    public class ManageTeamDto
    {
        public virtual List<TeamPlayerDto> Players { get; set; } = null!;

        public virtual TeamPlayerDto Captain { get; set; } = null!;

        [Display(Name = "Email drużynowy")]
        [Required(ErrorMessage = "Email jest wymagany.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Logo drużyny")]
        public byte[]? Logo { get; set; }

        [Display(Name = "Zdjęcie drużyny")]
        public byte[]? Photo { get; set; }

        [Display(Name = "Numer telefonu drużynowy")]
        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Opis drużyny")]
        public string? TeamDescription { get; set; }

        [Display(Name = "Strona internetowa")]
        public string? Website { get; set; }

        public virtual List<TeamPlayerDto> NewPlayers { get; set; } = null!;

        public virtual List<TeamPlayerDto> RemovedPlayers { get; set; } = null!;

        public static explicit operator ManageTeamDto(ManagedTeamDataDto team)
        {
            return new ManageTeamDto
            {
                Captain = team.Captain,
                Email = team.Email,
                Logo = team.Logo,
                Photo = team.Photo,
                Phone = team.Phone,
                Players = team.Players,
                TeamDescription = team.TeamDescription,
                Website = team.Website
            };
        }
    }
}