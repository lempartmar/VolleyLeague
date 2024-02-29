namespace VolleyLeague.Entities.Models
{
    public partial class Comment : BaseEntity
    {
        public int ContentId { get; set; }

        public int AuthorId { get; set; }

        public string Content { get; set; } = null!;

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public int CommentLocationId { get; set; }

        public virtual User Author { get; set; } = null!;

        public virtual CommentLocation CommentLocation { get; set; } = null!;
    }
}