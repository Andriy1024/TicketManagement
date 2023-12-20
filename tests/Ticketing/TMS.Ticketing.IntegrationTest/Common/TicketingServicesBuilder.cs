using TMS.Common.Caching;
using TMS.Ticketing.Persistence;
using TMS.Ticketing.Infrastructure;
using TMS.MongoDB;

namespace TMS.Ticketing.IntegrationTest.Common;

public class TicketingServicesBuilder : ServicesBuilder<TicketingServicesBuilder>
{
    public TicketingServicesBuilder AddMongoConnection(string connectionString, string dbName)
    {
        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.DatabaseName)}",
            dbName);

        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.ConnectionString)}",
            connectionString);

        return this;
    }

    public TicketingServicesBuilder AddTicketingServices() 
    {
        ArgumentNullException.ThrowIfNull(Config);

        Services
            .AddInfrastructure(Config)
            .AddPersistenceServices(Config);

        return this;
    }

    public TicketingServicesBuilder SetFakeCache()
    {
        OverrideService<ICacheClient>(new FakeCacheClient());

        return this;
    }
}