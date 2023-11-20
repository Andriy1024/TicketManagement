using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.UseCases.Prices;

public sealed class CreatePriceCommand : IRequest<EventDetailsDto>
{
    public Guid EventId { get; set; }

    public decimal Amount { get; set; }

    public string Name { get; set; }
}

internal sealed class CreatePriceHandler : IRequestHandler<CreatePriceCommand, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepository;

    public CreatePriceHandler(IEventsRepository eventsRepository)
        => _eventsRepository = eventsRepository;

    public async Task<EventDetailsDto> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepository.GetRequiredAsync(request.EventId);

        @event.Prices.Add(new Price 
        {
            Id = Guid.NewGuid(),
            EventId = @event.Id,
            Amount = request.Amount,
            Name = request.Name
        });

        await _eventsRepository.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}
