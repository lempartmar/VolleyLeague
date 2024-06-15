using VolleyLeague.Entities.Models;

namespace VolleyLeague.Shared.Dtos.Discussion
{
    public partial class NewArticleDto
    {
        public string Content { get; set; } = null!;

        public string Title { get; set; } = null!;

        public byte[] Image { get; set; } = null!;

        public static implicit operator NewArticleDto(Article article)
        {
            return new NewArticleDto
            {
                Content = article.Content,
                Title = article.Title,
                Image = article.Image,
            };
        }
    }
}
