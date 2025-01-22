using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Application.Tasks.CommandHandlers.UpdateTask;
using TaskManager.Domain;
using TasktManager.Application.Tests.Models;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Tasks
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<TaskAggregate.ITaskRepository> _mockTaskRepository;
        private readonly Mock<IRedisRepository> _mockRedisRepository;
        private readonly Mock<ILogger<UpdateTaskCommandHandler>> _mockLogger;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _mockTaskRepository = new Mock<TaskAggregate.ITaskRepository>();
            _mockRedisRepository = new Mock<IRedisRepository>();
            _mockLogger = new Mock<ILogger<UpdateTaskCommandHandler>>();
            _handler = new UpdateTaskCommandHandler(_mockTaskRepository.Object, _mockRedisRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenTaskNotFound()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = Guid.NewGuid() };
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((TaskAggregate.Task)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada no sistema!!", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserIsNotCreator()
        {
            // Arrange
            var command = TaskCommandModelTest.UpdateTaskCommandDefault();
            var task = TaskModelTest.TaskDefault();

            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.ValidateCreatorTask(task.Id, command.CreatedByUserId)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Apenas o usuário criador da tarefa pode atualiza-la!!", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTaskAndReturnSuccess()
        {
            // Arrange
            var command = TaskCommandModelTest.UpdateTaskCommandDefault();
            var task = TaskModelTest.TaskDefault();

            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.ValidateCreatorTask(task.Id, command.CreatedByUserId)).ReturnsAsync(true);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskAggregate.Task>())).ReturnsAsync(task);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            _mockTaskRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskAggregate.Task>()), Times.Once);
            _mockRedisRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
