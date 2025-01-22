using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.CrossCutting.Extensions;
using TaskManager.Data;
using TaskManager.Domain.UserAggregate;

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

        public static IApplicationBuilder AddInitialData(this IApplicationBuilder app)
        {
            using var serviceScop = app.ApplicationServices.CreateScope();
            var context = serviceScop.ServiceProvider.GetService<TaskManagerContext>();

            AddInitialData(context);

            return app;
        }

        public static void AddInitialData(TaskManagerContext context)
        {
            var populated = context.Set<User>().Any();
            if (populated)
                return;

            context.Add(new User("admin", "admin@gmail.com", "Admin@1234", DateTime.Now));

            context.SaveChanges();
        }
    }
}
