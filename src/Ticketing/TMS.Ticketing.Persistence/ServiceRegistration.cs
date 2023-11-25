using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using TMS.Common.Interfaces;

using TMS.Ticketing.Domain;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Implementations;
using TMS.Ticketing.Persistence.StartupTask;

namespace TMS.Ticketing.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
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

        services.AddTransient(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        BsonClassMapInitializer.Initialize();

        return services
            .AddScoped<IStartupTask, MongoSchemaTask>()
            //.AddScoped<IStartupTask, MongoSeedTask>()
            .AddScoped<IVenuesRepository, VenuesRepository>()
            .AddScoped<IVenuesBookingRepository, VenuesBookingRepository>()
            .AddScoped<IEventsRepository, EventsRepository>()
            .AddScoped<ICartsRepository, CartsRepository>()
            .AddScoped<IOrdersRepository, OrdersRepository>()
            .AddScoped<ITicketsRepository, TicketsRepository>();
    }

    private static void ConfigureClassMapp()
    {
        
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : IEntity<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}