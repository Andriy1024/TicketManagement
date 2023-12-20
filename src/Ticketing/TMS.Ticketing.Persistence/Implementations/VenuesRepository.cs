using TMS.Common.Errors;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal class VenuesRepository : ChangeTrackableRepository<VenueEntity, Guid>, IVenuesRepository
{
    protected override string CollectionName => Collections.Venues;

    public VenuesRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents)
         : base(database, transactionScope, domainEvents) {}

    public async Task<VenueEntity> GetRequiredAsync(Guid venueId)
        => await GetAsync(venueId)
            ?? throw ApiError.NotFound($"Venue not found: {venueId}").ToException();
}