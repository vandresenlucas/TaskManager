using MediatR;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetTasks
{
    public class GetTasksCommand : IRequest<Result>
    {
        public Status? Status { get; set; }
    }
}
