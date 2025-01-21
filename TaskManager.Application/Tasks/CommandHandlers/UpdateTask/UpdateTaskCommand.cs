using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.CrossCutting.Contracts;
using TaskEntity = TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.UpdateTask
{
    [SwaggerSchema(Description = "Representa os dados necessários para atualizar uma tarefa.")]
    public class UpdateTaskCommand : IRequest<Result>
    {
        [SwaggerSchema(Description = "Id da tarefa a ser atualizada")]
        public Guid Id { get; set; }
        [SwaggerSchema(Description = "Novo título para a tarefa. Pode ser deixado em branco/nulo para não modificar.")]
        public string? Title { get; set; }
        [SwaggerSchema(Description = "Nova descrição para a tarefa. Pode ser deixada em branco/nulo para não modificar.")]
        public string? Description { get; set; }
        [SwaggerSchema(Description = "Novo status da tarefa. Pode ser deixado em branco/nulo para não modificar.")]
        public TaskEntity.Status? Status { get; set; }
        [SwaggerSchema(Description = "Id do usuário que criou a tarefa. Usado para validação de autorização.")]
        public Guid CreatedByUserId { get; set; }
    }
}
