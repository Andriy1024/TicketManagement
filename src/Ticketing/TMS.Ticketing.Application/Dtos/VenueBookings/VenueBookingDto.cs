using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class VenueBookingDto
{
    public Guid Id { get; set; }

    public Guid VenueId { get; set; }

    public Guid EventId { get; set; }

    public int BookingNumber { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public static VenueBookingDto Map(VenueBookingEntity entity) => new() 
    {
        Id = entity.Id,
        VenueId = entity.VenueId,
        EventId = entity.EventId,
        BookingNumber = entity.BookingNumber,
        Start = entity.Start,
        End = entity.End
    };
}