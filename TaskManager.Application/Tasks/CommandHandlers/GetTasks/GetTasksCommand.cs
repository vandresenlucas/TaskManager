using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.CrossCutting.Contracts;
using TaskManager.CrossCutting.Contracts.Bases;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Application.Tasks.CommandHandlers.GetTasks
{
    [SwaggerSchema(Description = "Método utilizado para obter tarefas, podendo filtrar pelo status.")]
    public class GetTasksCommand : PaginationBase, IRequest<Result>
    {
        [SwaggerSchema(Description = "Status das tarefas a serem recuperadas. Se não for fornecido, todas as tarefas serão retornadas.")]
        public Status? Status { get; set; }
    }
}
