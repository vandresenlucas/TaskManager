using MediatR;
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

            await _taskRepository.DeleteAsync(request.Id);

            return new Result(message: "Tarefa excluída com sucesso!!");
        }
    }
}
