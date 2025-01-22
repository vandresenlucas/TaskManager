using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Providers;

namespace TaskManager.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDatabase(configuration);
            services.ConfigureServices();
            services.ConfigureMediatr();
            services.ConfigureAuthentication(configuration);
            services.AddApiVersioning(opt => opt.ReportApiVersions = true);
            services.ConfigureSwagger();
            services.ConfigureRedis(configuration);

            return services;
        }

        public static IApplicationBuilder Configure(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.AddInitialData();

            return app;
        }
    }
}
