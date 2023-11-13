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
            .AddMongoRepository<Venue, Guid>()
            .AddMongoRepository<VenueBooking, Guid>()
            .AddMongoRepository<Event, Guid>()
            .AddMongoRepository<Cart, Guid>()
            .AddMongoRepository<Order, Guid>()
            .AddMongoRepository<Ticket, Guid>();
    }

    private static void ConfigureClassMapp() 
    {
        BsonClassMap.RegisterClassMap<Venue>(map =>
        {
            map.AutoMap();

            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<VenueBooking>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<Event>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<Cart>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
            map.UnmapProperty(x => x.Total);
        });

        BsonClassMap.RegisterClassMap<Order>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<Ticket>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : ICollectionEntry<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IMongoRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}