using MediatR;

namespace TaskManager.Application.Tasks.CommandHandlers.GetAllTaskCommand
{
    public class GetAllTasksCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }
}
