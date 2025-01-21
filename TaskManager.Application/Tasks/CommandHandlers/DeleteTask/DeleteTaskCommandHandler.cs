using MediatR;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var taskFound = await _taskRepository.GetByIdAsync(request.Id);

            if (taskFound == null)
                return new Result(false, "Tarefa não encontrada no sistema!!");

            if (!await _taskRepository.ValidateCreatorTask(taskFound.Id, request.CreatedByUserId))
                return new Result(false, "Apenas o usuário criador da tarefa pode excluí-la!!");

            await _taskRepository.DeleteAsync(request.Id);

            return new Result(message: "Tarefa excluída com sucesso!!");
        }
    }
}
