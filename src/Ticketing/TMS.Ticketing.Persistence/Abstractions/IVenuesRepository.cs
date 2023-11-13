using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface IVenuesRepository : IRepository<VenueEntity, Guid> { }
