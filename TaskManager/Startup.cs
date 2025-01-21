using Microsoft.OpenApi.Models;
using TaskManager.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddControllers();
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

            services.RegisterServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Configure(Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
                });
            }

            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
