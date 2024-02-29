namespace VolleyLeague.Entities.Models

{
    public partial class Position : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}