using VolleyLeague.Entities.Models;

namespace VolleyLeague.Repositories.Interfaces
{
    public interface IDefaultRepository
    {
        Task<List<Season>> GetArticles();
    }
}
