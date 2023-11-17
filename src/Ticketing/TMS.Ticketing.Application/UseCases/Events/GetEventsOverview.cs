namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class GetEventsOverview : IRequest<IEnumerable<EventOverviewDto>>
{
}

public sealed class GetEventsOverviewHandler : IRequestHandler<GetEventsOverview, IEnumerable<EventOverviewDto>>
{
    private readonly IEventsRepository _eventsRepo;

    public GetEventsOverviewHandler(IEventsRepository eventsRepo)
    {
        this._eventsRepo = eventsRepo;
    }

    public async Task<IEnumerable<EventOverviewDto>> Handle(GetEventsOverview request, CancellationToken cancellationToken)
    {
        var events = await _eventsRepo.GetAllAsync(cancellationToken);

        return events.Select(EventOverviewDto.Map).ToList();
    }   
}