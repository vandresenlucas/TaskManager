using MediatR;
using EntityTask = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetTasks
{
    public class GetTasksCommandHandler : IRequestHandler<GetTasksCommand, Result>
    {
        private readonly EntityTask.ITaskRepository _taskRepository;

        public GetTasksCommandHandler(EntityTask.ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(GetTasksCommand request, CancellationToken cancellationToken)
        {
            var tasks = request.Status != null ?
                await _taskRepository.GetByStatus((EntityTask.Status)request.Status) :
                await _taskRepository.GetAll();

            return new Result(response: tasks);
        }
    }
}
