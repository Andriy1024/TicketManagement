using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.VenueBookings;

public sealed class CreateVenueBookingCommand : IRequest<VenueBookingDto>, IValidatable
{
    public Guid VenueId { get; set; }

    public Guid EventId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.VenueId).NotEmpty();
            x.RuleFor(y => y.EventId).NotEmpty();

            x.RuleFor(y => y.Start)
             .NotEmpty();

            x.RuleFor(y => y.End)
             .NotEmpty()
             .GreaterThanOrEqualTo(x => x.Start);
        });
    }
}

internal sealed class CreateVenueBookingHandler : IRequestHandler<CreateVenueBookingCommand, VenueBookingDto>
{
    private readonly IVenuesBookingRepository _bookingRepo;
    private readonly IEventsRepository _eventsRepo;
    private readonly IVenuesRepository _venueRepo;

    public CreateVenueBookingHandler(
        IVenuesBookingRepository bookingRepo,
        IEventsRepository eventsRepo,
        IVenuesRepository venueRepo)
    {
        this._bookingRepo = bookingRepo;
        this._eventsRepo = eventsRepo;
        this._venueRepo = venueRepo;
    }

    public async Task<VenueBookingDto> Handle(CreateVenueBookingCommand request, CancellationToken cancellationToken)
    {
        var venue = await _venueRepo.GetRequiredAsync(request.VenueId);

        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        var venueBookings = await _bookingRepo.FindAsync(x => x.VenueId == request.VenueId);

        if (!IsDateRangeAvailable(venueBookings, request.Start, request.End))
        {
            throw ApiError
                .InvalidData("Requested date range is not available")
                .ToException();
        }

        var bookingNumber = venueBookings.Count == 0 ? 1
            : venueBookings.Max(x => x.BookingNumber) + 1;

        var newBooking = new VenueBookingEntity
        {
            Id = Guid.NewGuid(),
            VenueId = venue.Id,
            EventId = @event.Id,
            Start = request.Start,
            End = request.End,
            BookingNumber = bookingNumber
        };

        @event.Seats = venue.Sections
            .SelectMany(x => x.Seats)
            .Select(x => new EventSeat
            {
                SeatId = x.SeatId,
                State = SeatState.Available
            })
            .ToList();

        await _bookingRepo.AddAsync(newBooking);

        await _eventsRepo.UpdateAsync(@event);

        return VenueBookingDto.Map(newBooking);
    }

    private static bool IsDateRangeAvailable(IEnumerable<VenueBookingEntity> bookings, DateTime startDate, DateTime endDate)
    {
        // Check if there are no bookings that overlap with the specified date range
        bool isAvailable = !bookings.Any(booking =>
            (startDate >= booking.Start && startDate <= booking.End) ||
            (endDate >= booking.Start && endDate <= booking.End) ||
            (startDate <= booking.Start && endDate >= booking.End));

        return isAvailable;
    }
}