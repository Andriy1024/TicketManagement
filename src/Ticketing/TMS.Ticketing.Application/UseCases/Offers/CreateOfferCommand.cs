namespace TMS.Ticketing.Application.UseCases.Offers;

public sealed class CreateOfferCommand : ICommand<EventDetailsDto>, IValidatable
{
    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.EventId).NotEmpty();
            x.RuleFor(y => y.SeatId).NotEmpty();
            x.RuleFor(y => y.PriceId).NotEmpty();
        });
    }
}

internal sealed class CreateOfferHandler : IRequestHandler<CreateOfferCommand, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepository;

    public CreateOfferHandler(IEventsRepository eventsRepository)
        => _eventsRepository = eventsRepository;

    public async Task<EventDetailsDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepository.GetRequiredAsync(request.EventId);

        @event.AddOffer(request.PriceId, request.SeatId);

        await _eventsRepository.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}