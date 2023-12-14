using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Application.UseCases.Events;

public sealed class UpdateEventCommand : ICommand<EventDetailsDto>, IValidatable
{
    public Guid EventId { get; set; }

    public string Name { get; set; }

    public List<Detail>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.EventId).NotEmpty();

            x.RuleFor(y => y.Name).NotEmpty();

            x.RuleFor(y => y.Start)
             .NotEmpty()
             .GreaterThanOrEqualTo(DateTime.UtcNow.Date);

            x.RuleFor(y => y.End)
             .NotEmpty()
             .GreaterThanOrEqualTo(y => y.Start);
        });
    }
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

        @event.Update(
            request.Name,
            request.Details,
            request.Start,
            request.End);

        await _eventsRepo.UpdateAsync(@event);

        return EventDetailsDto.Map(@event);
    }
}