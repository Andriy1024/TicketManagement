using MongoDB.Driver;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class VenuesBookingRepository : MongoRepository<VenueBookingEntity, Guid>, IVenuesBookingRepository
{
    protected override string CollectionName => Collections.VenuesBooking;

    public VenuesBookingRepository(IMongoDatabase database) : base(database)
    {
    }
}