using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Repositories;

public interface IVenuesRepository : IRepository<VenueEntity, Guid> 
{
    Task<VenueEntity> GetRequiredAsync(Guid venueId);
}
