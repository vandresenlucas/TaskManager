using System.Security.Claims;

namespace TaskManager.Application.Authentication
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiryToken(string token);
    }
}
