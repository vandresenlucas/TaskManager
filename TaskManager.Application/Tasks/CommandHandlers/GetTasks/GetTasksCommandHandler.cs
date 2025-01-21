using MediatR;
using System.Linq.Expressions;
using TaskManager.CrossCutting.Contracts;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetTasks
{
    public class GetTasksCommandHandler : IRequestHandler<GetTasksCommand, Result>
    {
        private readonly TaskAggregate.ITaskRepository _taskRepository;

        public GetTasksCommandHandler(TaskAggregate.ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(GetTasksCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<TaskAggregate.Task, bool>> filterExpression = t => t.Status == request.Status;

            var tasks = request.Status != null ?
                await _taskRepository.GetByFilterExpressionPaginated(filterExpression, request.CurrentPage, request.PageSize) :
                await _taskRepository.GetAllPaginated(request.CurrentPage, request.PageSize);

            return new Result(response: tasks);
        }
    }
}
