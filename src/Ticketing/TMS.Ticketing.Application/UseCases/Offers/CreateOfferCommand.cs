using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.UseCases.Offers;

public sealed class CreateOfferCommand : IRequest<EventDetailsDto>
{
    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }
}

internal sealed class CreateOfferHandler : IRequestHandler<CreateOfferCommand, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepository;

    public CreateOfferHandler(IEventsRepository eventsRepository)
        => _eventsRepository = eventsRepository;

    public async Task<EventDetailsDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepository.GetRequiredAsync(request.EventId);

        @event.Offers.Add(new Offer
        {
            PriceId = request.PriceId,
            SeatId = request.SeatId,
        });

        await _eventsRepository.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}
