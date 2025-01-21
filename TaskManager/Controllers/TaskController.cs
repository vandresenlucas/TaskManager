using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Application.Tasks.CommandHandlers.DeleteTask;
using TaskManager.Application.Tasks.CommandHandlers.GetTasks;
using TaskManager.Application.Tasks.CommandHandlers.UpdateTask;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Controllers
{
    [Route("TaskManager/[controller]")]
    [Authorize("bearer")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TaskController> _logger;

        public TaskController(IMediator mediator, ILogger<TaskController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "AddTask")]
        [SwaggerOperation(
            Summary = "Adiciona uma nova tarefa ao sistema",
            Description = "Recebe os dados de uma nova tarefa (título, descrição, status e usuário criador) e a adiciona ao sistema.",
            OperationId = "AddTask",
            Tags = new[] { "Adiciona Tarefas" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                _logger.LogInformation($"UsuárioId: {loggedUserId} adicionando uma tarefa.");

                command.CreatedByUserId = Guid.Parse(loggedUserId);

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar tarefa: {ex.Message}");
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpGet(Name = "GetTasks")]
        [SwaggerOperation(
            Summary = "Obtém uma lista de tarefas com possibilidade de filtrar pelo status",
            Description = "Retorna uma lista de tarefas, com a possibilidade de filtrar pelo status (Pendentes, Em andamento ou Concluídas).",
            OperationId = "GetTasks",
            Tags = new[] { "Busca Tarefas" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTasks([FromQuery] Status? status, [FromBody] GetTasksCommand command)
        {
            try
            {
                _logger.LogInformation($"UsuárioId: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value} buscando tarefas.");

                command.Status = status;
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar tarefas: {ex.Message}");
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpPut(Name = "UpdateTask")]
        [SwaggerOperation(
            Summary = "Atualiza os detalhes de uma tarefa existente",
            Description = "Atualiza os detalhes de uma tarefa já existente, como título, descrição e status.",
            OperationId = "UpdateTask",
            Tags = new[] { "Atualiza Tarefas" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTask([FromQuery] Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                _logger.LogInformation($"UsuárioId: {loggedUserId} tentando atualizar tarefa. TaskId: {taskId}");

                command.Id = taskId;
                command.CreatedByUserId = Guid.Parse(loggedUserId);
                var result = await _mediator.Send(command);

                if (!result.Success)
                    _logger.LogError($"Tarefa não atualizada. Motivo: {result.Message}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar tarefa: {ex.Message}");
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpDelete(Name = "DeleteTask")]
        [SwaggerOperation(
            Summary = "Exclui uma tarefa do sistema",
            Description = "Exclui uma tarefa existente com base no seu ID. Obs.: Apenas o usuário que criou a tarefa, poderá excluí-la.",
            OperationId = "DeleteTask",
            Tags = new[] { "Excluí Tarefas" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTask([FromQuery] Guid taskId)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                _logger.LogInformation($"UsuárioId: {loggedUserId} tentando excluir tarefa. TaskId: {taskId}");

                var command = new DeleteTaskCommand { Id = taskId, CreatedByUserId = Guid.Parse(loggedUserId) };
                var result = await _mediator.Send(command);

                if (!result.Success)
                    _logger.LogError($"Tarefa não atualizada. Motivo: {result.Message}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }
    }
}
