using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class TeamPlayerShortDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Imię zawodnika jest wymagane.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Nazwisko zawodnika jest wymagane.")]
        public string LastName { get; set; } = null!;

        public int Height { get; set; }

        public string PositionName { get; set; }
    }
}
