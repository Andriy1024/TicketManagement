using TMS.Test.Common;

namespace TMS.Ticketing.IntegrationTest.UseCases.Venues.Common;

[CollectionDefinition(VenuesDatabaseCollection.Name)]
public class VenuesDatabaseCollection : ICollectionFixture<MongoDbFactory>
{
    public const string Name = "VenuesDatabaseCollection";

    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
