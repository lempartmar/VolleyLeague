using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyLeague.Entities.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public List<Credentials> Credentials { get; set; } = null!;
    }
}
