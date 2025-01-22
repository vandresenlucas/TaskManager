using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.Authentication.Services;
using TaskManager.CrossCutting.Configurations;

namespace TasktManager.Application.Tests.Authentication
{
    public class TokenServiceTests
    {
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _tokenConfiguration = new TokenConfiguration
            {
                Secret = "ThisIsASecretKeyForTestingWithALeast33Caracters",
                Minutes = 30
            };
            _tokenService = new TokenService(_tokenConfiguration);
        }

        [Fact]
        public void GenerateAccessTokenShouldReturnValidToken()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Act
            var token = _tokenService.GenerateAccessToken(claims);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret)),
                ValidateIssuerSigningKey = true
            }, out var validatedToken);

            Assert.NotNull(principal);
            Assert.IsType<JwtSecurityToken>(validatedToken);
        }

        [Fact]
        public void GenerateRefreshTokenShouldReturnRandomString()
        {
            // Act
            var refreshToken1 = _tokenService.GenerateRefreshToken();
            var refreshToken2 = _tokenService.GenerateRefreshToken();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(refreshToken1));
            Assert.False(string.IsNullOrWhiteSpace(refreshToken2));
            Assert.NotEqual(refreshToken1, refreshToken2); // Tokens should be unique
        }

        [Fact]
        public void GetPrincipalFromExpiryTokenShouldReturnPrincipalForValidToken()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var token = _tokenService.GenerateAccessToken(claims);

            // Act
            var principal = _tokenService.GetPrincipalFromExpiryToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.Equal("TestUser", principal.Identity.Name);
        }
    }
}