using TMS.Test.Common;

using TMS.Ticketing.Persistence;
using TMS.Ticketing.Application;

namespace TMS.Ticketing.IntegrationTest.Common;

public class TicketingServices : ServicesBuilder<TicketingServices>
{
    public TicketingServices AddMongoConnection(string connectionString)
    {
        return AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.ConnectionString)}",
            connectionString);
    }

    public TicketingServices AddTicketingServices() 
    {
        Services
            .AddApplicationServices(Config)
            .AddPersistenceServices(Config);

        return this;
    }
}