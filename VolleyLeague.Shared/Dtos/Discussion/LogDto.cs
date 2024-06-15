using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Shared.Dtos.Discussion
{
    public class LogDto
    {
        public int Id { get; set; }

        public string? Link { get; set; }

        public string Description { get; set; } = null!;

        public DateTime Date { get; set; }

        public PlayerSummaryDto? User { get; set; }
    }
}
