using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using TMS.Common.Interfaces;
using TMS.Ticketing.Domain;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Database;

namespace TMS.Ticketing.Persistence.Setup;

public static class MongoSetupExtensions
{
    public static IServiceCollection AddMongoServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services
            .AddOptions<MongoConfig>()
            .BindConfiguration(nameof(MongoConfig));

        var options = configuration.GetSection(nameof(MongoConfig)).Get<MongoConfig>()
            ?? throw new ArgumentNullException(nameof(MongoConfig));

        services.AddSingleton<IMongoClient>(provider => 
        {
            return new MongoClient(options.ConnectionString);
        });
            
        services.AddTransient<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        services.AddScoped<IStartupTask, MongoSchemaTask>();

        services.AddScoped<IStartupTask, MongoSeedDataTask>();

        return services;
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : IDocumentEntry<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IMongoRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}