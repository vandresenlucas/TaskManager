using FluentValidation;
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
            AssemblyScanner.FindValidatorsInAssembly(assembly)
                .ForEach(a => services.AddScoped(a.InterfaceType, a.ValidatorType));

            return services;
        }
    }
}
