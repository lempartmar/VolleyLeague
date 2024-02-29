using VolleyLeague.Entities.Dtos.Teams;

namespace VolleyLeague.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<List<SeasonDto>> GetAllSeasons();

        void CreateSeason(SeasonDto season);

        Task UpdateSeason(SeasonDto season);
    }
}
