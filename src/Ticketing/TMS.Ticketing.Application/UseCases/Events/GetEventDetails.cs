namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class GetEventDetails : IRequest<EventDetailsDto>
{
    public Guid EventId { get; set; }
}

public sealed class GetEventDetailsHandler : IRequestHandler<GetEventDetails, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepo;

    public async Task<EventDetailsDto> Handle(GetEventDetails request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        return EventDetailsDto.Map(@event);
    }
}
