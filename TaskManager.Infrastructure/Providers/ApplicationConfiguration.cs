using Microsoft.Extensions.DependencyInjection;
using TaskManager.Data.Repository;
using TaskManager.Domain.TaskAggregate;

namespace TaskManager.Infrastructure.Providers
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}
