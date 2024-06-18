using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities;
using VolleyLeague.Repositories.Interfaces;

namespace VolleyLeague.Repositories.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        public readonly VolleyballContext _dbContextProvider;

        public BaseRepository(VolleyballContext dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual DbSet<TEntity> Table => _dbContextProvider.Set<TEntity>();

        public async Task<TEntity> GetById(long id)
        {
            return await Table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public async Task<TEntity> InsertOrUpdate(TEntity entity)
        {
            TEntity existingEntity = await _dbContextProvider.FindAsync<TEntity>(entity.Id);

            if (existingEntity == null)
            {
                var e = _dbContextProvider.Add(entity);
                await SaveChangesAsync();
                return e.Entity;
            }
            else
            {
                _dbContextProvider.Entry(existingEntity).CurrentValues.SetValues(entity);
                await SaveChangesAsync();
                return existingEntity;
            }
        }



        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContextProvider.Entry(entity).State = EntityState.Modified;
            _dbContextProvider.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            await _dbContextProvider.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                AttachIfNot(entity);
            }
            Table.RemoveRange(entities);
            await _dbContextProvider.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContextProvider.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        protected void AttachIfNot(TEntity entity)
        {
            var entry = _dbContextProvider.ChangeTracker.Entries()
                .FirstOrDefault(ent => ent.Entity == entity);

            if (entry != null)
            {
                return;
            }
            Table.Attach(entity);
        }
        public IQueryable<TEntity> GetAllDescending()
        {
            return Table.OrderByDescending(x => x.Id);
        }
    }
}
