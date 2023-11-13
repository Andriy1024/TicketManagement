namespace TMS.Ticketing.API.Dtos.VenueBooking;

public class CreateVenueBookingDto
{
    public Guid VenueId { get; set; }

    public Guid EventId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}