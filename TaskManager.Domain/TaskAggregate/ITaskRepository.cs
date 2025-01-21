namespace TaskManager.Domain.TaskAggregate
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<IEnumerable<Task>> GetAll();
        Task<IEnumerable<Task>> GetByStatus(Status status);
        Task<bool> ValidateCreatorTask(Guid taskId, Guid userId);
    }
}
