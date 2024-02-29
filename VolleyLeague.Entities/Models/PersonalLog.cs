namespace VolleyLeague.Entities.Models
{
    public partial class PersonalLog : BaseEntity
    {
        public int UserId { get; set; }

        public int LogId { get; set; }

        public virtual Log Log { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}