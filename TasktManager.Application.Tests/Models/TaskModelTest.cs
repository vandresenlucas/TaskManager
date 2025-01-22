using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Models
{
    public static class TaskModelTest
    {
        public static TaskAggregate.Task TaskDefault()
        {
            var task = new TaskAggregate.Task(
                "New Task",
                "Task Description",
                TaskAggregate.Status.InProgress,
                Guid.NewGuid(),
                DateTime.Now);

            task.Id = Guid.NewGuid();

            return task;
        }
    }
}
