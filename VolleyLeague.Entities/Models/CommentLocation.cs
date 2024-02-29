namespace VolleyLeague.Entities.Models
{
    public partial class CommentLocation : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}