using TaskManager.Domain;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Data
{
    public class Repository<TEntity> : IRepository<User>
    {
        protected readonly TaskManagerContext _context;

        public Repository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User entity)
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

        public Task<User> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
