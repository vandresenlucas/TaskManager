using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TaskManager.Application.Authentication.CommandHandlers;
using TaskManager.CrossCutting.Contracts;

namespace TaskManager.Controllers.Authenticate.V1
{
    //[Route("TaskManager/[controller]")]
    //[ApiController]
    [ApiVersion("1")]
    [ApiController]
    [Authorize("bearer")]
    [Route("api/v{version:apiVersion}/task-manager/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Realiza login do usuário",
            Description = "Recebe as credenciais de login do usuário (email e senha) e retorna um token de autenticação.",
            OperationId = "AuthenticationUser",
            Tags = new[] { "Autenticação" }
        )]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthenticationCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }
    }
}
