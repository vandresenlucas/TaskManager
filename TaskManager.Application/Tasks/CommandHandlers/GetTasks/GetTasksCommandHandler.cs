using MediatR;
using System.Linq.Expressions;
using TaskManager.CrossCutting.Contracts;
using TaskManager.CrossCutting.Contracts.Responses;
using TaskManager.Domain;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetTasks
{
    public class GetTasksCommandHandler : IRequestHandler<GetTasksCommand, Result>
    {
        private readonly TaskAggregate.ITaskRepository _taskRepository;
        private readonly IRedisRepository _redisRepository;

        public GetTasksCommandHandler(TaskAggregate.ITaskRepository taskRepository, IRedisRepository redisRepository)
        {
            _taskRepository = taskRepository;
            _redisRepository = redisRepository;
        }

        public async Task<Result> Handle(GetTasksCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            var cachedData = await _redisRepository.GetAsync<PaginatedResponse<TaskAggregate.Task>>(cacheKey);

            if (cachedData != null)
                return new Result(response: cachedData);

            Expression<Func<TaskAggregate.Task, bool>> filterExpression = t => t.Status == request.Status;

            var tasks = request.Status != null ?
                await _taskRepository.GetByFilterExpressionPaginated(filterExpression, request.CurrentPage, request.PageSize) :
                await _taskRepository.GetAllPaginated(request.CurrentPage, request.PageSize);

            await _redisRepository.SetAsync(cacheKey, tasks);

            return new Result(response: tasks);
        }
    }
}
