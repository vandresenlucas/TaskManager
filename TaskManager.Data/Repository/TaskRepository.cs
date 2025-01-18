using TaskManager.Domain.UserAggregate;

namespace TaskManager.Data.Repository
{
    public class TaskRepository : Repository<User>
    {
        public TaskRepository(TaskManagerContext context) : base(context)
        {
        }
    }
}
