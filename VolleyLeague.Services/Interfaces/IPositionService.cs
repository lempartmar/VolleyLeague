
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public interface IPositionService
    {
        Task<List<PositionDto>> GetAllPositions();

        Task CreatePosition(PositionDto position);

        Task UpdatePosition(PositionDto position);

        Task<bool> DeletePosition(int id);
    }

}

