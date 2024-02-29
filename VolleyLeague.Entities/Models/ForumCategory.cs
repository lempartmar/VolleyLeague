namespace VolleyLeague.Entities.Models
{
    public partial class ForumCategory : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<ForumTopic> Topics { get; set; } = new List<ForumTopic>();
    }
}