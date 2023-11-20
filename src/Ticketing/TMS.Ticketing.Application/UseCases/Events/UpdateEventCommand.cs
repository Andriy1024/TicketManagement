using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class UpdateEventCommand : IRequest<EventDetailsDto>
{
    public Guid EventId { get; set; }

    public string Name { get; set; }

    public List<KeyValePair>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}

internal sealed class UpdateEventHandler : IRequestHandler<UpdateEventCommand, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepo;

    public UpdateEventHandler(IEventsRepository eventsRepo)
    {
        _eventsRepo = eventsRepo;
    }

    public async Task<EventDetailsDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        @event.Name = request.Name;
        @event.Details = request.Details;
        @event.Start = request.Start;
        @event.End = request.End;

        await _eventsRepo.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}