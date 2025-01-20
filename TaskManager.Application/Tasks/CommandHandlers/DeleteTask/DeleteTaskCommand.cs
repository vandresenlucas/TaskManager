using MediatR;

namespace TaskManager.Application.Tasks.CommandHandlers.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}
