using MediatR;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.UpdateTaskCommand
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskFound = await _taskRepository.GetByIdAsync(request.Id);

            if (taskFound == null)
                return new Result(false, "Tarefa não encontrada no sistema!!");

            if (!String.IsNullOrEmpty(request.Title))
                taskFound.Title = request.Title;

            if (!String.IsNullOrEmpty(request.Description))
                taskFound.Description = request.Description;

            if(request.Status != null)
                taskFound.Status = (Status)request.Status;

            var updatedTask = await _taskRepository.UpdateAsync(taskFound);

            return new Result(response: updatedTask);
        }
    }
}
