using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Repositories;

public interface IVenuesBookingRepository : IRepository<VenueBookingEntity, Guid> 
{
}