using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface IVenuesBookingRepository : IRepository<VenueBookingEntity, Guid> { }