using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Teams
{
    public class TypedUserDto
    {
        public int UserId { get; set; }
        public string TemporaryVote { get; set; }
        public int CorrectResultsCount { get; set; }
        public int PerfectResultsCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}