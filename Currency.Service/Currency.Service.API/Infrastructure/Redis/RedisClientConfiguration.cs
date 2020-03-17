using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Currency.Service.API.Infrastructure.Redis
{
    public static class RedisClientConfiguration
    {
        public static IServiceCollection AddRedisClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ConnectionMultiplexer>(serviceProvider =>
            {
                var redis = ConnectionMultiplexer.Connect(configuration["RedisConnectionString"]);

                return redis;
            });

            services.AddScoped<IDatabase>(serviceProvider =>
            {
                var redis = serviceProvider.GetRequiredService<ConnectionMultiplexer>();

                return redis.GetDatabase();
            });

            return services;
        }

    }
}