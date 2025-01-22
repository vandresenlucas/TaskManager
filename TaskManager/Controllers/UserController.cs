using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TaskManager.Application.Users.CommandHandlers;
using TaskManager.CrossCutting.Contracts;

namespace TaskManager.Controllers
{
    [Route("TaskManager/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "AddUser")]
        [SwaggerOperation(
            Summary = "Cadastra um novo usuário",
            Description = "Realiza o cadastro de um novo usuário fazendo as validações necessárias pertinentes.",
            OperationId = "AddUser",
            Tags = new[] { "Cadastro de Usuários" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            try
            {
                _logger.LogInformation($"Iniciando cadastro para o email: {command.Email}.");

                var result = await _mediator.Send(command);

                if (!result.Success)
                    _logger.LogError($"Erro ao realizar cadastro: {result.Message}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar usuário: {ex.Message}");
                return BadRequest(new Result(false, ex.Message));
            }
        }
    }
}
