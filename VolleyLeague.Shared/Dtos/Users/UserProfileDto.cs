using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? BirthYear { get; set; }
        public string? City { get; set; }
        public string? PersonalInfo { get; set; }
        public byte[]? Photo { get; set; }
        public bool? Gender { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? JerseyNumber { get; set; }
        public int? BlockRange { get; set; }
        public int? AttackRange { get; set; }
        public string? TeamName { get; set; }
        public string? VolleyballIdol { get; set; }
        public string? Hobby { get; set; }
        public string? PositionName { get; set; }
        public int? PositionId { get; set; }

        // cast operator User to UserProfileDto
        public static explicit operator UserProfileDto(User user)
        {
            return new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthYear = user.BirthYear,
                City = user.City,
                PersonalInfo = user.PersonalInfo,
                Gender = user.Gender,
                Photo = user.Photo,
                Height = user.Height,
                Weight = user.Weight,
                JerseyNumber = user.JerseyNumber,
                BlockRange = user.BlockRange,
                AttackRange = user.AttackRange,
                VolleyballIdol = user.VolleyballIdol,
                Hobby = user.Hobby,
                PositionName = user.Position?.Name,
                PositionId = user.PositionId
            };
        }

    }
}
