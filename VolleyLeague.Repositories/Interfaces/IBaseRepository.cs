﻿namespace VolleyLeague.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        TEntity Insert(TEntity entity);

        IQueryable<TEntity> GetAll();

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> GetById(long id);

        Task<TEntity> UpdateAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task Delete(TEntity entity);

        Task SaveChangesAsync();

        Task RemoveRange(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetAllDescending();
    }
}
