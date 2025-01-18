using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<User> Products { get; set; }
    }
}
