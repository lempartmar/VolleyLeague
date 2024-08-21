using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class SeasonDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa sezonu jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa sezonu nie może przekraczać 100 znaków")]
        public string Name { get; set; }

        public static explicit operator SeasonDto(Season season)
        {
            return new SeasonDto
            {
                Id = season.Id,
                Name = season.Name
            };
        }

        public static explicit operator Season(SeasonDto seasonDto)
        {
            return new Season
            {
                Id = seasonDto.Id,
                Name = seasonDto.Name
            };
        }
    }
}
