using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Application.Tasks.CommandHandlers.DeleteTask;
using TaskManager.Domain;
using TasktManager.Application.Tests.Models;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Tasks
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<TaskAggregate.ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IRedisRepository> _redisRepositoryMock;
        private readonly Mock<ILogger<DeleteTaskCommandHandler>> _loggerMock;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _taskRepositoryMock = new Mock<TaskAggregate.ITaskRepository>();
            _redisRepositoryMock = new Mock<IRedisRepository>();
            _loggerMock = new Mock<ILogger<DeleteTaskCommandHandler>>();
            _handler = new DeleteTaskCommandHandler(
                _taskRepositoryMock.Object,
                _redisRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task HandleTaskNotFoundReturnsFailureResult()
        {
            // Arrange
            var command = TaskCommandModelTest.DeleteTaskCommandDefault(Guid.NewGuid());

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.Id))
                .ReturnsAsync((TaskAggregate.Task)null); 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada no sistema!!", result.Message);
        }

        [Fact]
        public async Task HandleUserIsNotTaskCreatorReturnsFailureResult()
        {
            // Arrange
            var command = TaskCommandModelTest.DeleteTaskCommandDefault(Guid.NewGuid());
            var task = TaskModelTest.TaskDefault();

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.Id))
                .ReturnsAsync(task); 

            _taskRepositoryMock
                .Setup(repo => repo.ValidateCreatorTask(task.Id, command.CreatedByUserId))
                .ReturnsAsync(false); 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Apenas o usuário criador da tarefa pode excluí-la!!", result.Message);
        }

        [Fact]
        public async Task HandleDeleteTaskSuccess()
        {
            // Arrange
            var command = TaskCommandModelTest.DeleteTaskCommandDefault(Guid.NewGuid());
            var task = TaskModelTest.TaskDefault();

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.Id))
                .ReturnsAsync(task); 

            _taskRepositoryMock
                .Setup(repo => repo.ValidateCreatorTask(task.Id, command.CreatedByUserId))
                .ReturnsAsync(true); 

            _taskRepositoryMock
                .Setup(repo => repo.DeleteAsync(command.Id))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            var logMessage = string.Empty;

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Tarefa excluída com sucesso!!", result.Message);
            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(command.Id), Times.Once);
            _redisRepositoryMock.Verify(redis => redis.DeleteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
