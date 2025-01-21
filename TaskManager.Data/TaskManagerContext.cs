using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.UserAggregate;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskEntity.Task> Tasks { get; set; }
    }
}
