using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.UseCases.Prices;

public sealed class CreatePriceCommand : IRequest<EventDetailsDto>, IValidatable
{
    public Guid EventId { get; set; }

    public decimal Amount { get; set; }

    public string Name { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.EventId).NotEmpty();
            x.RuleFor(y => y.Amount).GreaterThan(0);
            x.RuleFor(y => y.Name).NotEmpty();
        });
    }
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

        @event.AddPrice(request.Name, request.Amount);

        await _eventsRepository.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}
