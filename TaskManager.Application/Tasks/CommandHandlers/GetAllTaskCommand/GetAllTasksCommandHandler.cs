using MediatR;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetAllTaskCommand
{
    public class GetAllTasksCommandHandler : IRequestHandler<GetAllTasksCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(GetAllTasksCommand request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAll();

            return new Result(response: tasks);
        }
    }
}
