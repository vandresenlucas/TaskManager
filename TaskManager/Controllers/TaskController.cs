using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Application.Tasks.CommandHandlers.DeleteTask;
using TaskManager.Application.Tasks.CommandHandlers.GetAllTask;
using TaskManager.Application.Tasks.CommandHandlers.UpdateTask;
using TaskManager.Domain.TaskAggregate;

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

        [HttpGet(Name = "GetTasks")]
        public async Task<IActionResult> Get([FromQuery] Status? status)
        {
            try
            {
                var command = new GetAllTasksCommand { Status = status };
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpPut(Name = "UpdateTask")]
        public async Task<IActionResult> Put([FromQuery] Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            try
            {
                command.Id = taskId;
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpDelete(Name = "DeleteTask")]
        public async Task<IActionResult> Delete([FromQuery] Guid taskId)
        {
            try
            {
                var command = new DeleteTaskCommand { Id = taskId };
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
