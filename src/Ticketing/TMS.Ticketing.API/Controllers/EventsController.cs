using Microsoft.AspNetCore.Mvc;

using MediatR;
using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.UseCases.Events;

namespace TMS.Ticketing.API.Controllers;

[Route("api/events")]
[ApiController]
public sealed class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
        => this._mediator = mediator;

    [HttpGet("overview")]
    public Task<IEnumerable<EventOverviewDto>> GetEventsOverviewAsync(CancellationToken token)
        => _mediator.Send(new GetEventsOverview(), token);

    [HttpGet("{eventId}")]
    public Task<EventDetailsDto> GetEventDetailsAsync(Guid eventId, CancellationToken token)
        => _mediator.Send(new GetEventDetails() { EventId = eventId }, token);

    [HttpPost]
    public Task<EventDetailsDto> CreateEventAsync([FromBody] CreateEventCommand command)
        => _mediator.Send(command);

    [HttpPut]
    public Task<EventDetailsDto> UpdateEventAsync([FromBody] UpdateEventCommand command)
        => _mediator.Send(command);

    [HttpDelete("{eventId}")]
    public Task<Unit> DeleteEventAsync(Guid eventId)
        => _mediator.Send(new DeleteEventCommand() { EventId = eventId });
}