using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

using TMS.Caching.Abstractions;
using TMS.Caching.Implementations;

namespace TMS.Caching;

public static class ServiceRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Redis").Get<RedisConfiguration>();
        
        if (options == null)
            throw new ArgumentNullException("Redis options.");

        services.AddTransient<SystemTextJsonSerializer>();
      
        services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(options);

        services.AddSingleton<ICoreCacheClient, CoreCacheClient<DB0>>();

        return services;
    }
}