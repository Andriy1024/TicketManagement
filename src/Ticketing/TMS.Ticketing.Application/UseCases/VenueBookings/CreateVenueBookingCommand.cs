using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.VenueBookings;

public sealed class CreateVenueBookingCommand : ICommand<VenueBookingDto>, IValidatable
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
             .GreaterThan(x => x.Start);
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
        _bookingRepo = bookingRepo;
        _eventsRepo = eventsRepo;
        _venueRepo = venueRepo;
    }

    public async Task<VenueBookingDto> Handle(CreateVenueBookingCommand request, CancellationToken cancellationToken)
    {
        var venue = await _venueRepo.GetRequiredAsync(request.VenueId);

        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        var venueBookings = await _bookingRepo.FindAsync(x => x.VenueId == request.VenueId);

        var newBooking = VenueBookingEntity.Create(
            venueBookings,
            request.Start, request.End,
            venue, @event);

        await _bookingRepo.AddAsync(newBooking);

        await _eventsRepo.UpdateAsync(@event);

        return VenueBookingDto.Map(newBooking);
    }
}