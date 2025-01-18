namespace TaskManager.Domain.TaskAggregate
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<IEnumerable<Task>> GetAll();
    }
}
