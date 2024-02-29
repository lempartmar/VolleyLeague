using VolleyLeague.Entities.Models;

namespace VolleyLeague.Entities.Dtos.Teams
{
    public class PositionDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public static implicit operator Position(PositionDto positionDto)
        {
            return new Position
            {
                Id = positionDto.Id,
                Name = positionDto.Name
            };
        }

        public static explicit operator PositionDto(Position position)
        {
            return new PositionDto
            {
                Id = position.Id,
                Name = position.Name
            };
        }
    }
}
