using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using TMS.Common.Interfaces;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Tickets;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Persistence.Setup;

internal class MongoSchemaTask : IStartupTask
{
    private readonly IMongoDatabase _database;

    public MongoSchemaTask(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Verify collections successfully created
        _ = _database.GetCollection<Venue>(Venue.Collection);
        _ = _database.GetCollection<Event>(Event.Collection);
        _ = _database.GetCollection<Cart>(Cart.Collection);
        _ = _database.GetCollection<Order>(Order.Collection);
        _ = _database.GetCollection<Ticket>(Ticket.Collection);

        var venuesBooking = _database.GetCollection<VenueBooking>(VenueBooking.Collection);

        var indexes = venuesBooking.Indexes.List().ToList();

        var indexName = "VenueBookingUniqueIndex";

        if (indexes.All(index => index["name"] != (BsonValue)indexName))
        {
            var venueBookingUniqueIndex = Builders<VenueBooking>.IndexKeys
                .Combine(
                    Builders<VenueBooking>.IndexKeys.Ascending(x => x.VenueId),
                    Builders<VenueBooking>.IndexKeys.Ascending(x => x.BookingNumber)
                );

            await venuesBooking.Indexes.CreateOneAsync(
                new CreateIndexModel<VenueBooking>(venueBookingUniqueIndex,
                    new CreateIndexOptions
                    {
                        Unique = true,
                        Name = "VenueBookingUniqueIndex"
                    }
                )
            );
        }
    }
}