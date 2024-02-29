using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;

namespace VolleyLeague.Repositories
{
    public class DefaultRepository : IDefaultRepository
    {
        private readonly VolleyballContext _context;

        public DefaultRepository(VolleyballContext context)
        {
            _context = context;
        }

        public async Task<List<Season>> GetArticles()
        {
            var result = await _context.Seasons.OrderByDescending(s => s.Id).ToListAsync();
            return result;
        }

    }
}
