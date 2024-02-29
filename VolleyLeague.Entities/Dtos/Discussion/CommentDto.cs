using VolleyLeague.Entities.Dtos.Discussion;

namespace Volleyball.DTO.Discussion
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public AuthorInfoDto Author { get; set; } = null!;
    }
}
