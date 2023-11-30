using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

using TMS.Common.Caching;

namespace TMS.Caching.Redis;

public static class ServiceRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Redis").Get<RedisConfiguration>();
        
        _ = options ?? throw new ArgumentNullException("Redis options.");

        return services
            .AddTransient<SystemTextJsonSerializer>()
            .AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(options)
            .AddSingleton<ICoreCacheClient, CoreCacheClient<DB0>>();
    }
}