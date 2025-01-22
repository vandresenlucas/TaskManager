using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Application.Tasks.CommandHandlers.DeleteTask;
using TaskManager.Application.Tasks.CommandHandlers.GetTasks;
using TaskManager.Application.Tasks.CommandHandlers.UpdateTask;
using TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Models
{
    public static class TaskCommandModelTest
    {
        public static AddTaskCommand AddTaskCommandDefault()
            => new AddTaskCommand
            {
                Title = "New Task",
                Description = "Task Description",
                Status = Status.InProgress
            };

        public static DeleteTaskCommand DeleteTaskCommandDefault(Guid userId)
            => new DeleteTaskCommand
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = userId
            };

        public static GetTasksCommand GetTaskCommandDefault()
            => new GetTasksCommand 
            { 
                Status = Status.Pending, 
                CurrentPage = 1, 
                PageSize = 10 
            };

        public static GetTasksCommand GetTaskCommandWhithoutStatus()
            => new GetTasksCommand
            {
                CurrentPage = 1,
                PageSize = 10
            };

        public static UpdateTaskCommand UpdateTaskCommandDefault()
            => new UpdateTaskCommand 
            { 
                Id = Guid.NewGuid(), 
                CreatedByUserId = Guid.NewGuid() 
            };

        public static UpdateTaskCommand UpdateTaskCommandWithModifications()
            => new UpdateTaskCommand
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid()
            };
    }
}
