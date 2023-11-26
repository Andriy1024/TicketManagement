namespace TMS.Ticketing.Application.UseCases.Events;

public sealed record GetEventDetails(Guid EventId) : IRequest<EventDetailsDto>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.EventId).NotEmpty());
    }
};

internal sealed class GetEventDetailsHandler : IRequestHandler<GetEventDetails, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepo;

    public GetEventDetailsHandler(IEventsRepository eventsRepo)
    {
        _eventsRepo = eventsRepo;
    }

    public async Task<EventDetailsDto> Handle(GetEventDetails request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        return EventDetailsDto.Map(@event);
    }
}
