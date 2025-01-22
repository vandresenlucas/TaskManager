using Moq;
using System.Security.Claims;
using TaskManager.Application.Authentication.CommandHandlers;
using TaskManager.Application.Authentication.Services;
using TaskManager.CrossCutting.Configurations;
using TaskManager.Domain.UserAggregate;
using TasktManager.Application.Tests.Models;

namespace TasktManager.Application.Tests;

public class AuthenticationCommandHandlerTests
{
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly TokenConfiguration _tokenConfiguration;
    private readonly AuthenticationCommandHandler _handler;

    public AuthenticationCommandHandlerTests()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenConfiguration = new TokenConfiguration { Minutes = 30 };

        _handler = new AuthenticationCommandHandler(
            _tokenConfiguration,
            _tokenServiceMock.Object,
            _userRepositoryMock.Object
        );
    }

    [Fact]
    public async Task HandleShouldReturnErrorWhenUserNotFound()
    {
        // Arrange
        var command = new AuthenticationCommand { Email = "test@test", Password = "password" };
        _userRepositoryMock.Setup(repo => repo.ValidateCredentials(command))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Usuário não encontrado ou senha incorreta!!", result.Message);
    }

    [Fact]
    public async Task HandleShouldReturnSuccessWhenCredentialsAreValid()
    {
        // Arrange
        var user = UserModelTest.UserDefault();
        var command = new AuthenticationCommand { Email = "test@example.com", Password = "password" };

        _userRepositoryMock.Setup(repo => repo.ValidateCredentials(
            It.Is<UserCredentials>(
                c => c.Email == "test@example.com" && c.Password == "password")))
            .ReturnsAsync(user);

        _tokenServiceMock.Setup(service => service.GenerateAccessToken(It.IsAny<List<Claim>>()))
            .Returns("accessToken");

        _tokenServiceMock.Setup(service => service.GenerateRefreshToken())
            .Returns("refreshToken");

        _userRepositoryMock.Setup(repo => repo.UpdateRefreshToken(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Logado com sucesso!!", result.Message);
        Assert.Equal("accessToken", result.Response);
    }

    [Fact]
    public async Task HandleShouldGenerateAccessTokenWithCorrectClaims()
    {
        // Arrange
        var user = UserModelTest.UserDefault();
        var command = new AuthenticationCommand { Email = "test@example.com", Password = "password" };

        _userRepositoryMock.Setup(repo => repo.ValidateCredentials(
            It.Is<UserCredentials>(
                c => c.Email == "test@example.com" && c.Password == "password")))
            .ReturnsAsync(user);

        var capturedClaims = new List<Claim>();

        _tokenServiceMock.Setup(service => service.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>()))
            .Callback<IEnumerable<Claim>>(claims => capturedClaims = claims.ToList())
            .Returns("accessToken");

        _tokenServiceMock.Setup(service => service.GenerateRefreshToken())
            .Returns("refreshToken");

        _userRepositoryMock.Setup(repo => repo.UpdateRefreshToken(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Collection(capturedClaims,
            claim => Assert.Equal(ClaimTypes.Name, claim.Type),
            claim => Assert.Equal(ClaimTypes.Email, claim.Type),
            claim => Assert.Equal("UserId", claim.Type)
        );

        Assert.Equal(user.FullName, capturedClaims[0].Value);
        Assert.Equal(user.Email, capturedClaims[1].Value);
        Assert.Equal(user.Id.ToString(), capturedClaims[2].Value);
    }

    [Fact]
    public async Task HandleShouldUpdateUserRefreshToken()
    {
        // Arrange
        var user = UserModelTest.UserDefault();
        var command = new AuthenticationCommand { Email = "test@example.com", Password = "password" };

        _userRepositoryMock.Setup(repo => repo.ValidateCredentials(It.Is<UserCredentials>(
                c => c.Email == "test@example.com" && c.Password == "password")))
            .ReturnsAsync(user);

        _tokenServiceMock.Setup(service => service.GenerateAccessToken(It.IsAny<List<Claim>>()))
            .Returns("accessToken");

        _tokenServiceMock.Setup(service => service.GenerateRefreshToken())
            .Returns("refreshToken");

        User updatedUser = null;

        _userRepositoryMock.Setup(repo => repo.UpdateRefreshToken(It.IsAny<User>()))
            .Callback<User>(u => updatedUser = u)
            .ReturnsAsync(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("refreshToken", updatedUser.RefreshToken);
        Assert.True(updatedUser.ExpirationDate > DateTime.Now);
    }
}
