using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;

namespace TaskManager.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly TaskManagerContext _context;

        public Repository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                var result = await _context.AddAsync(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));
                _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == entity.Id);

            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
