namespace VolleyLeague.Entities.Models
{
    public class TeamImage : BaseEntity
    {
        public int TeamId { get; set; }
        public byte[] Image { get; set; } = null!;
        public string ImageType { get; set; } = null!;

        public virtual Team Team { get; set; } = null!;
    }
}