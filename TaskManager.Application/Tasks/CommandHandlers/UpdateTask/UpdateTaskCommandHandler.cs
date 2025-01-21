using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
    {
        private readonly TaskAggregate.ITaskRepository _taskRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly ILogger<UpdateTaskCommandHandler> _logger;

        public UpdateTaskCommandHandler(TaskAggregate.ITaskRepository taskRepository, IRedisRepository redisRepository, ILogger<UpdateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _redisRepository = redisRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            var taskFound = await _taskRepository.GetByIdAsync(request.Id);

            if (taskFound == null)
                return new Result(false, "Tarefa não encontrada no sistema!!");

            if (!await _taskRepository.ValidateCreatorTask(taskFound.Id, request.CreatedByUserId))
                return new Result(false, "Apenas o usuário criador da tarefa pode atualiza-la!!");

            var updatedTask = await UpdateTask(request, taskFound);
            _logger.LogInformation($"Tarefa atualizada com sucesso. Id: {updatedTask.Id}.");

            await _redisRepository.DeleteAsync(cacheKey);

            return new Result(response: updatedTask);
        }

        private async Task<TaskAggregate.Task> UpdateTask(UpdateTaskCommand request, TaskAggregate.Task task)
        {
            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                task.Description = request.Description;

            if (request.Status != null)
                task.Status = (TaskAggregate.Status)request.Status;

            task.UpdatedDate = DateTime.Now;

            return await _taskRepository.UpdateAsync(task);
        }
    }
}
