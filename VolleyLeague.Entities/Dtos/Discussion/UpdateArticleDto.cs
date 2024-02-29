namespace VolleyLeague.Entities.Dtos.Discussion
{
    public partial class UpdateArticleDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string Title { get; set; } = null!;

        public byte[] Image { get; set; } = null!;
    }
}
