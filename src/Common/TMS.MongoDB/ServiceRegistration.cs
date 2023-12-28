using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using TMS.Common.Interfaces;
using TMS.MongoDB.Repositories;
using TMS.MongoDB.Transactions;

namespace TMS.MongoDB;

public static class ServiceRegistration
{
    public static IServiceCollection AddMongoDBServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection(nameof(MongoConfig)).Get<MongoConfig>()
            ?? throw new ArgumentNullException(nameof(MongoConfig));

        return services
            .AddOptions<MongoConfig>()
            .BindConfiguration(nameof(MongoConfig))
            .Services
            .AddScoped<MongoTransactionScope>()
            .AddScoped<MongoTransactionScopeFactory>()
            .AddSingleton<IMongoClient>(provider => new MongoClient(options.ConnectionString))
            .AddTransient(provider => provider
                .GetRequiredService<IMongoClient>()
                .GetDatabase(options.DatabaseName)
            );
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : IEntity<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}
