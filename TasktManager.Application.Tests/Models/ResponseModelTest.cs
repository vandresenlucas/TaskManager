using TaskManager.CrossCutting.Contracts.Responses;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Models
{
    public static class ResponseModelTest
    {
        public static PaginatedResponse<TaskAggregate.Task> PaginatedResponseDefault()
            => new PaginatedResponse<TaskAggregate.Task>(
                new List<TaskAggregate.Task>(),
                0,
                10,
                1);

        public static PaginatedResponse<TaskAggregate.Task> PaginatedResponseWithTasks()
        {
            var tasks = new List<TaskAggregate.Task>();

            tasks.Add(TaskModelTest.TaskDefault());

            return new PaginatedResponse<TaskAggregate.Task>(
                tasks,
                0,
                10,
                1);
        }

    }
}
