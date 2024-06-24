using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Shared.Dtos.Discussion
{
    public partial class ArticleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Zawartość jest wymagana")]
        public string Content { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Zdjęcie jest wymagane")]
        public byte[] Image { get; set; } = null!;

        //public virtual AuthorInfoDto Author { get; set; } = null!;
    }
}
