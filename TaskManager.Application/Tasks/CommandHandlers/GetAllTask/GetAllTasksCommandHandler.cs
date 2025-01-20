using MediatR;
using EntityTask = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetAllTask
{
    public class GetAllTasksCommandHandler : IRequestHandler<GetAllTasksCommand, Result>
    {
        private readonly EntityTask.ITaskRepository _taskRepository;

        public GetAllTasksCommandHandler(EntityTask.ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(GetAllTasksCommand request, CancellationToken cancellationToken)
        {
            var tasks = request.Status != null ?
                await _taskRepository.GetByStatus((EntityTask.Status)request.Status) :
                await _taskRepository.GetAll();

            return new Result(response: tasks);
        }
    }
}
