using MediatR;
using System.Security.Claims;
using TaskManager.Application.Authentication.Services;
using TaskManager.CrossCutting.Configurations;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Application.Authentication.CommandHandlers
{
    public class AuthenticationCommandHandler(
        TokenConfiguration tokenOptions, ITokenService tokenService, IUserRepository userRepository) : IRequestHandler<AuthenticationCommand, Result>
    {
        private readonly TokenConfiguration _tokenOptions = tokenOptions;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.ValidateCredentials(request);

            if (user == null)
                return new Result(false, "Usuário não encontrado ou senha incorreta!!");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var createDate = DateTime.Now;
            var expirationDate = createDate.AddMinutes(_tokenOptions.Minutes);

            user.RefreshToken = refreshToken;
            user.ExpirationDate = expirationDate;

            user = await _userRepository.UpdateRefreshToken(user);

            return new Result(message: "Logado com sucesso!!", response: accessToken);
        }
    }
}
