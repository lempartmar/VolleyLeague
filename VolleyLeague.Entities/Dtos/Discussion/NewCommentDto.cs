using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Entities.Dtos.Discussion
{
    public class NewCommentDto
    {
        [MaxLength(500, ErrorMessage = "Komentarz przekracza limit 500 znaków.")]
        [Required(ErrorMessage = "Komentarz nie może być pusty.")]
        public string Content { get; set; } = null!;

        public int ContentLocationId { get; set; }
    }
}
