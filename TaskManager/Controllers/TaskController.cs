using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application;
using TaskManager.Application.Tasks.CommandHandlers.AddTaskCommand;
using TaskManager.Application.Tasks.CommandHandlers.GetAllTaskCommand;

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

        [HttpPost(Name = "AddTask")]
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

        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> Get([FromQuery] Guid userId)
        {
            try
            {
                var command = new GetAllTasksCommand { UserId = userId };
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
