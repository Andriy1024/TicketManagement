using MongoDB.Bson;
using MongoDB.Driver;

using TMS.Common.Interfaces;

using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Setup;

internal sealed class MongoSchemaTask : IStartupTask
{
    private readonly IMongoDatabase _database;
    private readonly IVenuesRepository _venuesRepo;
    private readonly IVenuesBookingRepository _venuesBookingRepo;
    private readonly IEventsRepository _eventsRepo;
    private readonly ICartsRepository _cartsRepo;
    private readonly IOrdersRepository _ordersRepo;
    private readonly ITicketsRepository _ticketsRepo;

    public MongoSchemaTask(
        IMongoDatabase database, 
        IVenuesRepository venuesRepo, 
        IVenuesBookingRepository venuesBookingRepo, 
        IEventsRepository eventsRepo, 
        ICartsRepository cartsRepo, 
        IOrdersRepository ordersRepo, 
        ITicketsRepository ticketsRepo)
    {
        _database = database;
        _venuesRepo = venuesRepo;
        _venuesBookingRepo = venuesBookingRepo;
        _eventsRepo = eventsRepo;
        _cartsRepo = cartsRepo;
        _ordersRepo = ordersRepo;
        _ticketsRepo = ticketsRepo;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Verify repositories successfully created
        var fakeId = Guid.NewGuid();
        
        _ = await _venuesRepo.GetAsync(fakeId);
        _ = await _venuesBookingRepo.GetAsync(fakeId);
        _ = await _eventsRepo.GetAsync(fakeId);
        _ = await _cartsRepo.GetAsync(fakeId);
        _ = await _ordersRepo.GetAsync(fakeId);
        _ = await _ticketsRepo.GetAsync(fakeId);

        var venuesBooking = _database.GetCollection<VenueBookingEntity>(Collections.VenuesBooking);

        var indexes = venuesBooking.Indexes.List().ToList();

        var indexName = "VenueBookingUniqueIndex";

        if (indexes.All(index => index["name"] != (BsonValue)indexName))
        {
            var venueBookingUniqueIndex = Builders<VenueBookingEntity>.IndexKeys
                .Combine(
                    Builders<VenueBookingEntity>.IndexKeys.Ascending(x => x.VenueId),
                    Builders<VenueBookingEntity>.IndexKeys.Ascending(x => x.BookingNumber)
                );

            await venuesBooking.Indexes.CreateOneAsync(
                new CreateIndexModel<VenueBookingEntity>(venueBookingUniqueIndex,
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