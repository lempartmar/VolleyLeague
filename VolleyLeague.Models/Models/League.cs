namespace VolleyLeague.Entities.Models
{
    public partial class League : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}