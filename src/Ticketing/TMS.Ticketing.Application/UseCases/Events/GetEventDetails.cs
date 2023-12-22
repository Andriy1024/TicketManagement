using TMS.Ticketing.Application.Helpers;

namespace TMS.Ticketing.Application.UseCases.Events;

public sealed record GetEventDetails(Guid EventId) : IQuery<EventDetailsDto>, IValidatable, ICachable
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.EventId).NotEmpty());
    }

    public string GetCacheKey() => CacheKeys.GetEventKey(EventId);
}

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
