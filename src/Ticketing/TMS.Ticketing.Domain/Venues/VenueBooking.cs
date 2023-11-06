namespace TMS.Ticketing.Domain.Venues;

/// <summary>
/// (VenueId, BookingNumber) - Forms Unique Constraint to handle concurrency, and prevent venue booking that has overlapping DateTime Range.
/// </summary>
public sealed class VenueBooking
{
    public required int BookingId { get; set; }

    public required int VenueId { get; set; }

    public required int EventId { get; set; }

    public required int BookingNumber { get; set; }

    public required DateTime Start { get; set; }

    public required DateTime End { get; set; }
}