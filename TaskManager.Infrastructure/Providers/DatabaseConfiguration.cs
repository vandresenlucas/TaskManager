using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Data;

namespace TaskManager.Infrastructure.Providers
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskManagerContext>(options => options.UseInMemoryDatabase("TaskManagerDatabase"));
            services.AddScoped<DbContext, TaskManagerContext>();

            return services;
        }
    }
}
