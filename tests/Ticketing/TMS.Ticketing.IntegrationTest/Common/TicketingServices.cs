using TMS.Ticketing.Persistence;
using TMS.Ticketing.Application;

namespace TMS.Ticketing.IntegrationTest.Common;

public class TicketingServices : ServicesBuilder<TicketingServices>
{
    public TicketingServices AddMongoConnection(string connectionString, string dbName)
    {
        // "mongodb://mongo:mongo@127.0.0.1:54359/"

        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.DatabaseName)}",
            dbName);

        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.ConnectionString)}",
            connectionString);

        return this;
    }

    public TicketingServices AddTicketingServices() 
    {
        ArgumentNullException.ThrowIfNull(Config);

        Services
            .AddApplicationServices(Config)
            .AddPersistenceServices(Config);

        return this;
    }
}