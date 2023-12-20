using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TMS.MongoDB;

using TMS.Ticketing.Infrastructure.Transactions;
using TMS.Ticketing.Persistence.Implementations;
using TMS.Ticketing.Persistence.Sessions;
using TMS.Ticketing.Persistence.StartupTask;

namespace TMS.Ticketing.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        BsonClassMapInitializer.Initialize();

        return services
            .AddMongoDBServices(configuration)
            .AddScoped<IStartupTask, MongoSchemaTask>()
            //.AddScoped<IStartupTask, MongoSeedTask>()
            .AddScoped<IVenuesRepository, VenuesRepository>()
            .AddScoped<IVenuesBookingRepository, VenuesBookingRepository>()
            .AddScoped<IEventsRepository, EventsRepository>()
            .AddScoped<ICartsRepository, CartsRepository>()
            .AddScoped<IOrdersRepository, OrdersRepository>()
            .AddScoped<ITicketsRepository, TicketsRepository>()
            .AddScoped<ITransactionManager, MongoTransactionManager>();
    }
}