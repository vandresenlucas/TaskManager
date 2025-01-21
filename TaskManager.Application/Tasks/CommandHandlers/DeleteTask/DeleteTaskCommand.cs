using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.CrossCutting.Contracts;

namespace TaskManager.Application.Tasks.CommandHandlers.DeleteTask
{
    [SwaggerSchema(Description = "Representa os dados para excluir uma tarefa")]
    public class DeleteTaskCommand : IRequest<Result>
    {
        [SwaggerSchema(Description = "Id da tarefa a ser excluída")]
        public Guid Id { get; set; }
        [SwaggerSchema(Description = "ID do usuário que criou a tarefa, utilizado para validação")]
        public Guid CreatedByUserId { get; set; }
    }
}
