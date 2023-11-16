using MongoDB.Driver;
using TMS.Common.Errors;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class VenuesRepository : MongoRepository<VenueEntity, Guid>, IVenuesRepository
{
    protected override string CollectionName => Collections.Venues;

    public VenuesRepository(IMongoDatabase database) : base(database)
    {
    }

    public async Task<VenueEntity> GetRequiredAsync(Guid venueId)
    {
        return await GetAsync(venueId)
            ?? throw AppError.NotFound($"Venue not found: {venueId}").ToException();
    }
}