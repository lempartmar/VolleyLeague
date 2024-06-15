using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class TeamPlayerDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Imię zawodnika jest wymagane.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Nazwisko zawodnika jest wymagane.")]
        public string LastName { get; set; } = null!;

        public string? Email { get; set; }

        public int Height { get; set; }

        public int JerseyNumber { get; set; }

        public int PositionId { get; set; }

        public string? PositionName { get; set; }

        public bool Gender { get; set; }

        public bool IsRegisteredUser { get; set; }

        public static explicit operator TeamPlayerDto(User user)
        {
            return new TeamPlayerDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Height = user.Height ?? 0,
                JerseyNumber = user.JerseyNumber ?? 0,
                PositionId = user.PositionId,
                Gender = user.Gender,
                IsRegisteredUser = true,
                Email = ""
            };
        }

        public static explicit operator TeamPlayerDto(TeamPlayer teamPlayer)
        {
            return new TeamPlayerDto
            {
                Id = teamPlayer.PlayerId,
                FirstName = teamPlayer.Player.FirstName,
                LastName = teamPlayer.Player.LastName,
                Height = teamPlayer.Player.Height ?? 0,
                JerseyNumber = teamPlayer.Player.JerseyNumber ?? 0,
                PositionId = teamPlayer.Player.PositionId,
                Gender = teamPlayer.Player.Gender,
                IsRegisteredUser = true,
                Email = ""
            };
        }

    }
}