using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TaskManager.Infrastructure.Providers
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskManager",
                    Version = "v1"
                });

                c.EnableAnnotations();

                //Inclui o XML do TaskManager
                var mainXml = Path.Combine(AppContext.BaseDirectory, "TaskManager.xml");
                if (File.Exists(mainXml))
                {
                    c.IncludeXmlComments(mainXml);
                }

                //Inclui o XML das anotações da TaskManager.Application.dll 
                var applicationDllXml = Path.Combine(AppContext.BaseDirectory, "TaskManager.Application.xml");
                if (File.Exists(applicationDllXml))
                {
                    c.IncludeXmlComments(applicationDllXml);
                }
            });

            return services;
        }
    }
}
