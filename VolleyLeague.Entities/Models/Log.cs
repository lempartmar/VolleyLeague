namespace VolleyLeague.Entities.Models
{
    public partial class Log : BaseEntity
    {
        public string? Link { get; set; }

        public string Description { get; set; } = null!;

        public DateTime Date { get; set; }

        public bool Admin { get; set; }

        public virtual ICollection<PersonalLog> PersonalLogs { get; set; } = new List<PersonalLog>();
    }
}