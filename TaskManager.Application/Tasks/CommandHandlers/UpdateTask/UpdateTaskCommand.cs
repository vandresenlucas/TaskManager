using MediatR;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.UpdateTask
{
    public class UpdateTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskEntity.Status? Status { get; set; }
    }
}
