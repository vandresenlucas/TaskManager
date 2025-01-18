using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Infrastructure.Providers
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("TaskManager.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
