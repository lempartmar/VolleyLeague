namespace VolleyLeague.Entities.Dtos.Discussion
{
    public class AuthorInfoDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public byte[]? Photo { get; set; }
    }
}
