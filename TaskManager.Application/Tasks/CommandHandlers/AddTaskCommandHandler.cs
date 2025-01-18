using MediatR;
using TaskManager.Domain.TaskAggregate;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMediator _mediator;

        public AddTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator)
        {
            _taskRepository = taskRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity.Task task = request;

            //fazer validação por título

            var newTask = await _taskRepository.AddAsync(task);

            return new Result();
        }
    }
}
