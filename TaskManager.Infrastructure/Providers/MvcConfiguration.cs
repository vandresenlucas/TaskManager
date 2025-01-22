using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Infrastructure.Providers
{
    public static class MvcConfiguration
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
                options.Conventions.Add(new VersionPrefixConventionOption()));

            return services;
        }
    }
}
