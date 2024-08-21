using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class RoundDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kolejki jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa kolejki nie może być dłuższa niż 100 znaków")]
        public string Name { get; set; } = null!;

        public int SeasonId { get; set; }

        public static explicit operator RoundDto(Round round)
        {
            return new RoundDto
            {
                Id = round.Id,
                Name = round.Name,
                SeasonId = round.SeasonId
            };
        }

        public static explicit operator Round(RoundDto roundDto)
        {
            return new Round
            {
                Id = roundDto.Id,
                Name = roundDto.Name,
                SeasonId = roundDto.SeasonId
            };
        }
    }
}
