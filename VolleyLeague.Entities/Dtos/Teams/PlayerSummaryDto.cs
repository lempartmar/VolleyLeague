namespace VolleyLeague.Entities.Dtos.Teams
{
    public class PlayerSummaryDto
    {
        public int Id { get; set; }

        public string Name
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public byte[]? Photo { get; set; }

        public int? Height { get; set; }

        public string PositionName { get; set; } = null!;

        public int? JerseyNumber { get; set; }

        public int TotalMvpCount { get; set; }

    }
    }
