using MediatR;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.AddTaskCommand
{
    public class AddTaskCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskEntity.Status Status { get; set; }

        public static implicit operator TaskEntity.Task(AddTaskCommand command)
        {
            if (command == null)
                return null;

            return new(
                command.Title,
                command.Description,
                command.Status,
                DateTime.UtcNow);
        }
    }
}
