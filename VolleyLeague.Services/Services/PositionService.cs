using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.Services.Services
{
    public class PositionService : IPositionService
    {
        private readonly ILogger<PositionService> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Position> _positionRepository;

        public PositionService(
            IMapper mapper,
            IBaseRepository<Position> positionRepository
            )
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
        }

        public async Task<List<PositionDto>> GetAllPositions()
        {
            try
            {
                var positionsAll = await _positionRepository.GetAll().ToListAsync();
                var positionsAllDto = _mapper.Map<List<PositionDto>>(positionsAll);

                return positionsAllDto;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task CreatePosition(PositionDto position)
        {
            var newPosition = _mapper.Map<Position>(position);
            await _positionRepository.InsertAsync(newPosition);
        }

        public async Task UpdatePosition(PositionDto position)
        {
            var positionToUpdate = await _positionRepository.GetById(position.Id);
            if (positionToUpdate == null)
            {
                throw new KeyNotFoundException(ServicesConsts.League_not_found);
            }
            positionToUpdate.Name = position.Name;
            await _positionRepository.UpdateAsync(positionToUpdate);
        }

        public async Task<bool> DeletePosition(int id)
        {
            var result = true;
            var position = await _positionRepository.GetById(id);

            if (position != null)
            {
                try
                {
                    await _positionRepository.Delete(position);
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }

            return false;
        }
    }
}
