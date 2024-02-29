namespace VolleyLeague.Entities.Models
{
    public partial class SportsVenue : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}