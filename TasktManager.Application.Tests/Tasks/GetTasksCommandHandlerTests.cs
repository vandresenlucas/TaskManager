using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Tasks.CommandHandlers.GetTasks;
using TaskManager.CrossCutting.Contracts.Responses;
using TaskManager.Domain;
using TasktManager.Application.Tests.Models;
using TaskAggregate = TaskManager.Domain.TaskAggregate;

namespace TasktManager.Application.Tests.Tasks
{
    public class GetTasksCommandHandlerTests
    {
        private readonly Mock<TaskAggregate.ITaskRepository> _mockTaskRepository;
        private readonly Mock<IRedisRepository> _mockRedisRepository;
        private readonly Mock<ILogger<GetTasksCommandHandler>> _mockLogger;
        private readonly GetTasksCommandHandler _handler;

        public GetTasksCommandHandlerTests()
        {
            _mockTaskRepository = new Mock<TaskAggregate.ITaskRepository>();
            _mockRedisRepository = new Mock<IRedisRepository>();
            _mockLogger = new Mock<ILogger<GetTasksCommandHandler>>();
            _handler = new GetTasksCommandHandler(
                _mockTaskRepository.Object,
                _mockRedisRepository.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task HandleShouldReturnCachedDataWhenCacheIsAvailable()
        {
            // Arrange
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            var cachedTasks = ResponseModelTest.PaginatedResponseDefault();
            var command = TaskCommandModelTest.GetTaskCommandDefault();

            _mockRedisRepository
                .Setup(r => r.GetAsync<PaginatedResponse<TaskAggregate.Task>>(cacheKey))
                .ReturnsAsync(cachedTasks);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equivalent(result.Response, cachedTasks); 
            _mockRedisRepository.Verify(r => r.GetAsync<PaginatedResponse<TaskAggregate.Task>>(cacheKey), Times.Once);
            _mockTaskRepository.VerifyNoOtherCalls();
            _mockLogger.Verify(logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Dados apresentados do Redis")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFetchFromDatabaseWhenCacheIsNotAvailable()
        {
            // Arrange
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            _mockRedisRepository
                .Setup(r => r.GetAsync<PaginatedResponse<TaskAggregate.Task>>(cacheKey))
                .ReturnsAsync((PaginatedResponse<TaskAggregate.Task>)null);
            
            var tasks = ResponseModelTest.PaginatedResponseWithTasks();

            _mockTaskRepository
                .Setup(r => r.GetAllPaginated(1, 10))
                .ReturnsAsync(tasks);

            var command = TaskCommandModelTest.GetTaskCommandWhithoutStatus();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equivalent(result.Response, tasks);
            _mockRedisRepository.Verify(r => r.SetAsync(cacheKey, tasks), Times.Once);
            _mockTaskRepository.Verify(r => r.GetAllPaginated(1, 10), Times.Once);
            _mockLogger.Verify(logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Dados apresentados do banco de dados.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldReturnAllTasksWhenStatusIsNull()
        {
            // Arrange
            var cacheKey = $"{nameof(TaskAggregate.Task)}:{DateTime.Now.Date}";
            _mockRedisRepository
                .Setup(r => r.GetAsync<PaginatedResponse<TaskAggregate.Task>>(cacheKey))
                .ReturnsAsync((PaginatedResponse<TaskAggregate.Task>)null);

            var tasks = ResponseModelTest.PaginatedResponseDefault();

            _mockTaskRepository
                .Setup(r => r.GetAllPaginated(1, 10))
                .ReturnsAsync(tasks);

            var command = new GetTasksCommand { Status = null, CurrentPage = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equivalent(result.Response, tasks);
            _mockTaskRepository.Verify(r => r.GetAllPaginated(1, 10), Times.Once);
            _mockRedisRepository.Verify(r => r.SetAsync(cacheKey, tasks), Times.Once);
            _mockLogger.Verify(logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Dados apresentados do banco de dados.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
