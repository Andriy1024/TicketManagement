using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class VenueBookingDto
{
    public static VenueBookingDto Map(VenueBookingEntity entity) => new();
}