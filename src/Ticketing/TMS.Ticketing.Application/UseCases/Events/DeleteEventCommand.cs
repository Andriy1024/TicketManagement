namespace TMS.Ticketing.Application.UseCases.Events;

public sealed record DeleteEventCommand(Guid EventId) : ICommand<Unit>, IValidatable
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x => 
            x.RuleFor(y => y.EventId).NotEmpty());
    }
};

internal sealed class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Unit>
{
    private readonly IEventsRepository _eventsRepo;

    public DeleteEventHandler(IEventsRepository eventsRepo)
    {
        _eventsRepo = eventsRepo;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        @event.Delete();

        await _eventsRepo.DeleteAsync(@event);

        return Unit.Value;
    }
}