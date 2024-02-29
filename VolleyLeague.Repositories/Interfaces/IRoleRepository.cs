using VolleyLeague.Entities.Models;

namespace VolleyLeague.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();
    }
}
