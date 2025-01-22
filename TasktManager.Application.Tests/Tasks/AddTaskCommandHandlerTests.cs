using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Domain;
using TasktManager.Application.Tests.Models;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Tasks
{
    public class AddTaskCommandHandlerTests
    {
        private readonly Mock<TaskAggregate.ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IRedisRepository> _redisRepositoryMock;
        private readonly Mock<ILogger<AddTaskCommandHandler>> _loggerMock;
        private readonly AddTaskCommandHandler _handler;
        private readonly string _cacheKey;

        public AddTaskCommandHandlerTests()
        {
            _taskRepositoryMock = new Mock<TaskAggregate.ITaskRepository>();
            _redisRepositoryMock = new Mock<IRedisRepository>();
            _loggerMock = new Mock<ILogger<AddTaskCommandHandler>>();
            _handler = new AddTaskCommandHandler(_taskRepositoryMock.Object, _redisRepositoryMock.Object, _loggerMock.Object);
            _cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
        }

        [Fact]
        public async Task HandleShouldAddTaskAndInvalidateCacheWhenValidRequestIsProvided()
        {
            // Arrange
            var command = TaskCommandModelTest.AddTaskCommandDefault();

            var task = TaskModelTest.TaskDefault();

            _taskRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<TaskAggregate.Task>()))
                .ReturnsAsync(task);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Response);
            Assert.Equal(task, result.Response);

            _taskRepositoryMock.Verify(repo => repo.AddAsync(It.Is<TaskAggregate.Task>(t => t.Title == command.Title && t.Description == command.Description)), Times.Once);
            _redisRepositoryMock.Verify(redis => redis.DeleteAsync(_cacheKey), Times.Once);
            _loggerMock.Verify(logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Tarefa adicionada com sucesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
