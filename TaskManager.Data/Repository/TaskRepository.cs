using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Data.Repository
{
    public class TaskRepository : Repository<TaskEntity.Task>, TaskEntity.ITaskRepository
    {
        public TaskRepository(TaskManagerContext context) : base(context)
        {
        }
    }
}
