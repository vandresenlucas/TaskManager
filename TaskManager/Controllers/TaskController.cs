using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Application.Tasks.CommandHandlers.DeleteTask;
using TaskManager.Application.Tasks.CommandHandlers.GetTasks;
using TaskManager.Application.Tasks.CommandHandlers.UpdateTask;
using TaskManager.Domain.TaskAggregate;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Controllers
{
    [Route("TaskManager/[controller]")]
    [Authorize("bearer")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                command.CreatedByUserId = Guid.Parse(loggedUserId);

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<IActionResult> GetTasks([FromQuery] Status? status)
        {
            try
            {
                var command = new GetTasksCommand { Status = status };
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpPut(Name = "UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromQuery] Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                command.Id = taskId;
                command.CreatedByUserId = Guid.Parse(loggedUserId);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(false, ex.Message));
            }
        }

        [HttpDelete(Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromQuery] Guid taskId)
        {
            try
            {
                var loggedUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var command = new DeleteTaskCommand { Id = taskId, CreatedByUserId = Guid.Parse(loggedUserId) };
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
