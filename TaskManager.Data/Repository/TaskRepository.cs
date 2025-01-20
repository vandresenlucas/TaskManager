using Microsoft.EntityFrameworkCore;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Data.Repository
{
    public class TaskRepository : Repository<TaskEntity.Task>, TaskEntity.ITaskRepository
    {
        public TaskRepository(TaskManagerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskEntity.Task>> GetAll()
            => await _context.Set<TaskEntity.Task>().ToListAsync();

        public async Task<IEnumerable<TaskEntity.Task>> GetByStatus(TaskEntity.Status status)
            => await _context.Set<TaskEntity.Task>().Where(task => task.Status == status).ToListAsync();
    }
}
