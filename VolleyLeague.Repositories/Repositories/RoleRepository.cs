using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;

namespace VolleyLeague.Repositories.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly VolleyballContext _context;

        public RoleRepository(VolleyballContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await _context.Roles.ToListAsync();
            return result;
        }   
    }
}
