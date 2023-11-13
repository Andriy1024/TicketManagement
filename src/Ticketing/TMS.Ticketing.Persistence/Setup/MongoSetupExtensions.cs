using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using TMS.Common.Interfaces;

using TMS.Ticketing.Domain;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Tickets;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Database;
using TMS.Ticketing.Persistence.Implementations;

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
            
        services.AddTransient(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });
        
        ConfigureClassMapp();

        return services
            .AddScoped<IStartupTask, MongoSchemaTask>()
            .AddScoped<IStartupTask, MongoSeedTask>()
            .AddScoped<IVenuesRepository, VenuesRepository>()
            .AddScoped<IVenuesBookingRepository, VenuesBookingRepository>()
            .AddScoped<IEventsRepository, EventsRepository>()
            .AddScoped<ICartsRepository, CartsRepository>()
            .AddScoped<IOrdersRepository, OrdersRepository>()
            .AddScoped<ITicketsRepository, TicketsRepository>();
    }

    private static void ConfigureClassMapp() 
    {
        BsonClassMap.RegisterClassMap<VenueEntity>(map =>
        {
            map.AutoMap();

            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<VenueBookingEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<EventEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<CartEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
            map.UnmapProperty(x => x.Total);
        });

        BsonClassMap.RegisterClassMap<OrderEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<TicketEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : IEntity<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}