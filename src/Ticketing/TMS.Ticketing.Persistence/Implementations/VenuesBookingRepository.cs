using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class VenuesBookingRepository : ChangeTrackableRepository<VenueBookingEntity, Guid>, IVenuesBookingRepository
{
    protected override string CollectionName => Collections.VenuesBooking;

    public VenuesBookingRepository(IMongoDatabase database, IEntityChangeTracker domainEvents)
         : base(database, domainEvents) { }
}