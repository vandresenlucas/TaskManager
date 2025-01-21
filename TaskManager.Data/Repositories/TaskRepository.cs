using Microsoft.EntityFrameworkCore;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Data.Repository
{
    public class TaskRepository : Repository<TaskAggregate.Task>, TaskAggregate.ITaskRepository
    {
        public TaskRepository(TaskManagerContext context) : base(context) { }

        public async Task<IEnumerable<TaskAggregate.Task>> GetAll()
            => await _context.Set<TaskAggregate.Task>().ToListAsync();

        public async Task<IEnumerable<TaskAggregate.Task>> GetByStatus(TaskAggregate.Status status)
            => await _context.Set<TaskAggregate.Task>().Where(t => t.Status == status).ToListAsync();

        public async Task<bool> ValidateCreatorTask(Guid taskId, Guid userId)
            => await _context.Set<TaskAggregate.Task>().AnyAsync(t => t.Id == taskId && t.CreatedByUserId == userId);
    }
}
