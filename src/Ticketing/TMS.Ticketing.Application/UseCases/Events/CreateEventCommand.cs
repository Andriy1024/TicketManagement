using TMS.Common.Users;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class CreateEventCommand : IRequest<EventDetailsDto>
{
    public string Name { get; set; }

    public List<KeyValePair>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}

internal sealed class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDetailsDto>
{
    private readonly IEventsRepository _eventsRepo;

    private readonly IUserContext _userContext;

    public CreateEventHandler(IEventsRepository eventsRepo, IUserContext userContext)
    {
        _eventsRepo = eventsRepo;
        _userContext = userContext;
    }

    public async Task<EventDetailsDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Details = request.Details,
            Start = request.Start,
            End = request.End,
            CreatorId = _userContext.GetUser().Id
        };

        await _eventsRepo.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}