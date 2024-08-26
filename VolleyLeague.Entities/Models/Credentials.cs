namespace VolleyLeague.Entities.Models
{
    public class Credentials : BaseEntity
    {
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;

        public int? UserId { get; set; }  

        public Guid? AccountId { get; set; }  

        public virtual User User { get; set; } = null!;  

        public List<Role> Roles { get; set; } = new List<Role>(); 
    }
}
