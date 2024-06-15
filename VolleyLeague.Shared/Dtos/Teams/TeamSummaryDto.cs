using System.ComponentModel.DataAnnotations;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class TeamSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int LeagueId { get; set; }

        public byte[]? Logo { get; set; }

        public static explicit operator TeamSummaryDto(Team team)
        {
            return new TeamSummaryDto
            {
                Id = team.Id,
                Name = team.Name,
                LeagueId = team.LeagueId
            };
        }

    }
}
