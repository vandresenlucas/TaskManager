using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TaskManager.CrossCutting.Configurations.Options;

namespace TaskManager.Infrastructure.Providers
{
    public static class RedisConfiguration
    {
        public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisOptions = new RedisOptions();

            new ConfigureFromConfigurationOptions<RedisOptions>(
                configuration.GetSection("RedisOptions"))
                .Configure(redisOptions);

            configuration.GetSection("RedisOptions").Bind(redisOptions);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisOptions.ConnectionString; 
                options.InstanceName = redisOptions.InstanceName;
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse("localhost:6379", true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });

            return services;
        }
    }
}
