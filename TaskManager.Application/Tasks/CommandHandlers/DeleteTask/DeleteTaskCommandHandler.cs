using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
    {
        private readonly TaskAggregate.ITaskRepository _taskRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly ILogger<DeleteTaskCommandHandler> _logger;

        public DeleteTaskCommandHandler(TaskAggregate.ITaskRepository taskRepository, IRedisRepository redisRepository, ILogger<DeleteTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _redisRepository = redisRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            var taskFound = await _taskRepository.GetByIdAsync(request.Id);

            if (taskFound == null)
                return new Result(false, "Tarefa não encontrada no sistema!!");

            if (!await _taskRepository.ValidateCreatorTask(taskFound.Id, request.CreatedByUserId))
                return new Result(false, "Apenas o usuário criador da tarefa pode excluí-la!!");

            await _taskRepository.DeleteAsync(request.Id);
            _logger.LogInformation($"Tarefa excluida com sucesso. Id: {request.Id}.");

            await _redisRepository.DeleteAsync(cacheKey);

            return new Result(message: "Tarefa excluída com sucesso!!");
        }
    }
}
