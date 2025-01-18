using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application;
using TaskManager.Application.Tasks.CommandHandlers;

namespace TaskManager.Controllers
{
    [Route("TaskManager/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> Post([FromBody] AddTaskCommand command)
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
