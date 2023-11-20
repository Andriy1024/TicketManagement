namespace TMS.Ticketing.Application.UseCases.Events;

public sealed record DeleteEventCommand(Guid EventId) : IRequest<Unit>;

internal sealed class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Unit>
{
    private readonly IEventsRepository _eventsRepo;

    public DeleteEventHandler(IEventsRepository eventsRepo)
    {
        _eventsRepo = eventsRepo;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await _eventsRepo.DeleteAsync(request.EventId);

        return Unit.Value;
    }
}