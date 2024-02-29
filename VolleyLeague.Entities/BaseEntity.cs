using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

    }
}
