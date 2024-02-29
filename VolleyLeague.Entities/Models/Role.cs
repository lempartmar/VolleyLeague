namespace VolleyLeague.Entities.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public List<Credentials> Credentials { get; set; } = null!;
    }
}
