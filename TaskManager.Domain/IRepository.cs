using System.Linq.Expressions;
using TaskManager.CrossCutting.Contracts.Responses;

namespace TaskManager.Domain
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<PaginatedResponse<TEntity>> GetAllPaginated(int page, int pageSize);
        Task<PaginatedResponse<TEntity>> GetByFilterExpressionPaginated(Expression<Func<TEntity, bool>> filterExpression, int page, int pageSize);
    }
}
