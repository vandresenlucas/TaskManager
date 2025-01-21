using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.CrossCutting.Contracts.Responses;
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

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                _context.SaveChanges();
            }
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

        public async Task<PaginatedResponse<TEntity>> GetAllPaginated(int page, int pageSize)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();

            return new PaginatedResponse<TEntity>(
                items,
                totalCount,
                pageSize,
                page);
        }

        public async Task<PaginatedResponse<TEntity>> GetByFilterExpressionPaginated(Expression<Func<TEntity, bool>> filterExpression, int page, int pageSize)
        {
            var query = _context.Set<TEntity>().Where(filterExpression);
            var totalCount = await query.CountAsync();

            var totalRecords = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();

            return new PaginatedResponse<TEntity>(
                items,
                totalCount,
                pageSize,
                page);
        }
    }
}
