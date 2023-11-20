namespace TMS.Ticketing.Application.UseCases.Events;

public sealed record GetEventDetails(Guid EventId) : IRequest<EventDetailsDto>;

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
