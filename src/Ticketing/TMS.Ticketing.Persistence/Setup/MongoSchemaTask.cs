using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using TMS.Common.Interfaces;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Persistence.Setup;

internal class MongoSchemaTask : IStartupTask
{
    private readonly IMongoDatabase database;

    public MongoSchemaTask(IMongoDatabase database)
    {
        this.database = database;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Venue Booking Collection
        //await ConfigureVenueBookingAsycn();
        //await ConfigureVenueAsycn();

        // Venue Collection

        //var venueColl = database.GetCollection<VenueAggregate>(VenueAggregate.Collection);

        //var bookingColl = database.GetCollection<VenueBooking>(VenueBooking.Collection);

        //var pipeline = venueColl.Aggregate()
        //    .Lookup(
        //        foreignCollection: bookingColl,
        //        localField: o => o.Id,
        //        foreignField: p => p.VenueId,
        //        @as: (VenueAggregate o) => o.Bookings // This is the name of the property in the Order class where the joined data will be stored
        //    );

        //var result1 = pipeline.ToList();

        //var query = 
        //    from x in venueColl.AsQueryable()
        //    join y in bookingColl.AsQueryable()
        //        on x.Id equals y.VenueId
        //    select new { x, y };

        //var result2 = query.ToList();
    }

    private Task ConfigureVenueAsycn() 
    {
        BsonClassMap.RegisterClassMap<Venue>(map =>
        {
            map.AutoMap();

            map.MapIdProperty(x => x.Id);
        });

        _ = database.GetCollection<Venue>(Venue.Collection);

        return Task.CompletedTask;
    }

    private async Task ConfigureVenueBookingAsycn() 
    {
        BsonClassMap.RegisterClassMap<VenueBooking>(map =>
        {
            map.AutoMap();
            map.MapIdProperty(x => x.Id);
        });

        var collection = database.GetCollection<VenueBooking>(VenueBooking.Collection);

        var indexes = collection.Indexes.List().ToList();

        var indexName = "VenueBookingUniqueIndex";

        if (indexes.All(index => index["name"] != (BsonValue)indexName))
        {
            var venueBookingUniqueIndex = Builders<VenueBooking>.IndexKeys
                .Combine(
                    Builders<VenueBooking>.IndexKeys.Ascending(x => x.VenueId),
                    Builders<VenueBooking>.IndexKeys.Ascending(x => x.BookingNumber)
                );

            await collection.Indexes.CreateOneAsync(
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