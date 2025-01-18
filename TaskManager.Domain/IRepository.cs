namespace TaskManager.Domain
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
