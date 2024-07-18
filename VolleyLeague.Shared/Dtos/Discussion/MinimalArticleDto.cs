using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyLeague.Shared.Dtos.Discussion
{
    public class MinimalArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public byte[] Image { get; set; } = null!;
    }
}
