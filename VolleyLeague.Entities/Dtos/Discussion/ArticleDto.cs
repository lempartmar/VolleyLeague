namespace VolleyLeague.Entities.Dtos.Discussion
{
    public partial class ArticleDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public string Title { get; set; } = null!;

        public byte[] Image { get; set; } = null!;

        public virtual AuthorInfoDto Author { get; set; } = null!;
    }
}
