namespace VolleyLeague.Entities.Models

{
    public partial class Round : BaseEntity
    {
        public string Name { get; set; } = null!;

        public int SeasonId { get; set; }

        public virtual Season Season { get; set; } = null!;

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}