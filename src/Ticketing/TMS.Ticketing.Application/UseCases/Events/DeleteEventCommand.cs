namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class DeleteEventCommand : IRequest<Unit>
{
    public Guid EventId { get; set; }
}

internal sealed class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Unit>
{
    private readonly IEventsRepository _eventsRepo;

    public DeleteEventHandler(IEventsRepository eventsRepo)
    {
        this._eventsRepo = eventsRepo;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await _eventsRepo.DeleteAsync(request.EventId);

        return Unit.Value;
    }
}