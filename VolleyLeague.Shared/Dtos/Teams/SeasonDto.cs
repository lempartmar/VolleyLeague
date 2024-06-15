using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class SeasonDto
    {
        public int Id { get; set; }
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
