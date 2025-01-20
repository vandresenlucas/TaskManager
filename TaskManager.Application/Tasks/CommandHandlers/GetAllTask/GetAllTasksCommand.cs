using MediatR;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetAllTask
{
    public class GetAllTasksCommand : IRequest<Result>
    {
        public Status? Status { get; set; }
    }
}
