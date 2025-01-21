using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain;
using TaskManager.Domain.TaskAggregate;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.AddTask
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly ILogger<AddTaskCommandHandler> _logger;

        public AddTaskCommandHandler(ITaskRepository taskRepository, IRedisRepository redisRepository, ILogger<AddTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _redisRepository = redisRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            TaskAggregate.Task task = request;
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";

            //fazer validação por título

            var newTask = await _taskRepository.AddAsync(task);

            _logger.LogInformation($"Tarefa adicionada com sucesso. Id: {newTask.Id}.");

            await _redisRepository.DeleteAsync(cacheKey);

            return new Result(response: newTask);
        }
    }
}
