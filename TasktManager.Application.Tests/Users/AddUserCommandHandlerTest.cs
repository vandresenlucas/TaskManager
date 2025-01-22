using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.Application.Users.CommandHandlers;
using TaskManager.Domain.UserAggregate;
using TasktManager.Application.Tests.Models;

namespace TasktManager.Application.Tests;
public class AddUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<AddTaskCommandHandler>> _loggerMock;
    private readonly AddUserCommandHandler _handler;

    public AddUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<AddTaskCommandHandler>>();
        _handler = new AddUserCommandHandler(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleShouldReturnErrorWhenEmailAlreadyExists()
    {
        // Arrange
        var command = UserCommandModelTest.UserCommandDefault();

        _userRepositoryMock
            .Setup(repo => repo.VerifyUserExists(command.Email))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Email {command.Email} já está registrado no sistema!!", result.Message);
    }

    [Fact]
    public async Task HandleShouldReturnErrorWhenPasswordIsInvalid()
    {
        // Arrange
        var command = UserCommandModelTest.UserCommandWithPassword("teste123");

        _userRepositoryMock
            .Setup(repo => repo.VerifyUserExists(command.Email))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A senha deve ter no mínimo 8 caractere, letras maiúsculas, minúsculas, números e caracteres especiais!!", result.Message);
    }

    [Fact]
    public async Task HandleShouldReturnSuccessWhenUserIsAddedSuccessfully()
    {
        // Arrange
        var command = UserCommandModelTest.UserCommandDefault();

        var user = UserModelTest.UserDefault();

        _userRepositoryMock
            .Setup(repo => repo.VerifyUserExists(command.Email))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal(user, result.Response);
    }
}

