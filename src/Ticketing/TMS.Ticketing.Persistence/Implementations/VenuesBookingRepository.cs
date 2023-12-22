using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;
using TMS.Ticketing.Persistence.Sessions;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class VenuesBookingRepository : ChangeTrackableRepository<VenueBookingEntity, Guid>, IVenuesBookingRepository
{
    protected override string CollectionName => Collections.VenuesBooking;

    public VenuesBookingRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents)
         : base(database, transactionScope, domainEvents) { }
}