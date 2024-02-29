namespace VolleyLeague.Entities.Models
{
    public partial class Season : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
    }
}