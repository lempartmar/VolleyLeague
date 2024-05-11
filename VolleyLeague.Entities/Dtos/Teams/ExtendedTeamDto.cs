using VolleyLeague.Entities.Dtos.Users;

namespace VolleyLeague.Entities.Dtos.Teams
{
    public class ExtendedTeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsReportedToPlay { get; set; }

        public bool Accepted { get; set; }

        public string LeagueName { get; set; }

        public int LeagueId { get; set; } 

        public string Email { get; set; }

        public string Phone { get; set; }

        public int ChangeCount { get; set; }

        public decimal PointCorrection { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsEditing { get; set; } = false;

    }
}
