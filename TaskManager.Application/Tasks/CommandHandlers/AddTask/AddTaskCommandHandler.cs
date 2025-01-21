using MediatR;
using TaskManager.Domain.TaskAggregate;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.AddTask
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;

        public AddTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity.Task task = request;

            //fazer validação por título

            var newTask = await _taskRepository.AddAsync(task);

            return new Result(response: newTask);
        }
    }
}
