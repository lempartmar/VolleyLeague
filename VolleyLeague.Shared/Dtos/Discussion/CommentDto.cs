namespace VolleyLeague.Shared.Dtos.Discussion
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public AuthorInfoDto Author { get; set; } = null!;
    }
}
