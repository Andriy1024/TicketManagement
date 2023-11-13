using Microsoft.AspNetCore.Mvc;

using TMS.Common.Errors;

using TMS.Ticketing.API.Dtos.VenueBooking;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;

namespace TMS.Ticketing.API.Controllers;

[Route("api/venues")]
[ApiController]
public class VenueBookingsController : ControllerBase
{
    private readonly IMongoRepository<Event, Guid> _eventsRepo;
    private readonly IMongoRepository<Venue, Guid> _venueRepo;
    private readonly IMongoRepository<VenueBooking, Guid> _venueBookingRepo;

    public VenueBookingsController(
        IMongoRepository<Event, Guid> eventsRepo, 
        IMongoRepository<Venue, Guid> venueRepo, 
        IMongoRepository<VenueBooking, Guid> venueBookingRepo)
    {
        _eventsRepo = eventsRepo;
        _venueRepo = venueRepo;
        _venueBookingRepo = venueBookingRepo;
    }

    [HttpPost("book")]
    public async Task<IActionResult> CreateVenueBookingAsync([FromBody] CreateVenueBookingDto dto)
    {
        var venue = await _venueRepo.GetAsync(x => x.Id == dto.VenueId);

        if (venue == null)
        {
            throw AppError
                .NotFound("Venue was not found")
                .ToException();
        }

        var @event = await _eventsRepo.GetAsync(x => x.Id == dto.EventId);

        if (venue == null)
        {
            throw AppError
                .NotFound("Event was not found")
                .ToException();
        }

        var venueBookings = await _venueBookingRepo.FindAsync(x => x.VenueId == dto.VenueId);

        if (!IsDateRangeAvailable(venueBookings, dto.Start, dto.End))
        {
            throw AppError
                .InvalidData("Requested date range is not available")
                .ToException();
        }

        var bookingNumber = venueBookings.Max(x => x.BookingNumber);

        await _venueBookingRepo.AddAsync(new VenueBooking
        {
            Id = Guid.NewGuid(),
            VenueId = venue.Id,
            EventId = @event.Id,
            Start = dto.Start,
            End = dto.End,
            BookingNumber = bookingNumber++
        });

        @event.Seats = venue.Sections
            .SelectMany(x => x.Seats)
            .Select(x => new EventSeat
            {
                SeatId = x.SeatId,
                State = SeatState.Available
            })
            .ToList();

        return Ok();
    }

    private static bool IsDateRangeAvailable(IEnumerable<VenueBooking> bookings, DateTime startDate, DateTime endDate)
    {
        // Check if there are no bookings that overlap with the specified date range
        bool isAvailable = !bookings.Any(booking =>
            (startDate >= booking.Start && startDate <= booking.End) ||
            (endDate >= booking.Start && endDate <= booking.End) ||
            (startDate <= booking.Start && endDate >= booking.End));

        return isAvailable;
    }
}