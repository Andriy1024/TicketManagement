using TMS.Common.Errors;
using TMS.Ticketing.Domain.DateRanges;
using TMS.Ticketing.Domain.DomainEvents;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Domain.Venues;

/// <summary>
/// (VenueId, BookingNumber) - Forms Unique Constraint to handle concurrency, and prevent venue booking that has overlapping DateTime Range.
/// </summary>
public sealed class VenueBookingEntity : Entity, IEntity<Guid>, IDateRange
{
    public required Guid Id { get; set; }

    public required Guid VenueId { get; set; }

    public required Guid EventId { get; set; }

    public required int BookingNumber { get; set; }

    public required DateTime Start { get; set; }

    public required DateTime End { get; set; }

    public int Version { get; set; } = 1;

    public static VenueBookingEntity Create(
        IEnumerable<VenueBookingEntity> booked,
        DateTime start,
        DateTime end,
        VenueEntity venue,
        EventEntity @event)
    {
        if (!booked.IsDateRangeAvailable(start, end))
        {
            throw ApiError
                .InvalidData("Requested date range is not available")
                .ToException();
        }

        if (!@event.IsInRange(start, end))
        {
            throw ApiError
                .InvalidData("The booking date range is not within the event date range")
                .ToException();
        }

        var bookingNumber = booked.Any()
            ? booked.Max(x => x.BookingNumber) + 1
            : 1;

        var booking = new VenueBookingEntity
        {
            Id = Guid.NewGuid(),
            VenueId = venue.Id,
            EventId = @event.Id,
            Start = start,
            End = end,
            BookingNumber = bookingNumber
        };

        @event.CreateSeats(venue);

        booking.AddDomainEvent(new EntityCreated<VenueBookingEntity>(booking));

        return booking;
    }

    public (int Old, int New) IncreaseVersion() => (Version, ++Version);
}