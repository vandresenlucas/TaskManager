using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Tasks.CommandHandlers.GetTasks;

namespace TaskManager.Infrastructure.Providers
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("TaskManager.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTasksCommandHandler>());

            return services;
        }
    }
}
