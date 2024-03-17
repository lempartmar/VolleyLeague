using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyLeague.Services.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromFormAttribute : Attribute
    {
    }
}
