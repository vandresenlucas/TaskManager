using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Authentication.Services;
using TaskManager.Data.Repository;
using TaskManager.Domain.TaskAggregate;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Infrastructure.Providers
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
