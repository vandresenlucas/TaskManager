using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.AddTask
{
    [SwaggerSchema(Description = "Representa os dados para adicionar uma nova tarefa. Obs.: O Status é iniciado como 'Pending'")]
    public class AddTaskCommand : IRequest<Result>
    {
        [SwaggerSchema(Description = "Título da tarefa", Nullable = false)]
        public string Title { get; set; }
        [SwaggerSchema(Description = "Descrição detalhada da tarefa", Nullable = false)]
        public string Description { get; set; }
        [SwaggerSchema(Description = "Status atual da tarefa")]
        public TaskEntity.Status Status { get; set; }
        [SwaggerSchema(Description = "ID do usuário que criou a tarefa", Nullable = false)]
        public Guid CreatedByUserId { get; set; }

        public static implicit operator TaskEntity.Task(AddTaskCommand command)
        {
            if (command == null)
                return null;

            return new(
                command.Title,
                command.Description,
                command.Status,
                command.CreatedByUserId,
                DateTime.Now);
        }
    }
}
