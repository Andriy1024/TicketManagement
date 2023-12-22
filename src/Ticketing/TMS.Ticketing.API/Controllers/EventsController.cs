using Microsoft.AspNetCore.Mvc;

using MediatR;

using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.UseCases.Events;
using TMS.Ticketing.Application.UseCases.Prices;
using TMS.Ticketing.Application.UseCases.Offers;

namespace TMS.Ticketing.API.Controllers;

[Route("api/events")]
[ApiController]
public sealed class EventsController : ControllerBase
{
    #region Context

    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
        => _mediator = mediator;

    #endregion

    #region Events

    //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    [HttpGet("overview")]
    public Task<IEnumerable<EventOverviewDto>> GetEventsOverviewAsync(CancellationToken token)
        => _mediator.Send(new GetEventsOverview(), token);

    [HttpGet("{eventId}")]
    public Task<EventDetailsDto> GetEventDetailsAsync(Guid eventId, CancellationToken token)
        => _mediator.Send(new GetEventDetails(eventId), token);

    [HttpPost]
    public Task<EventDetailsDto> CreateEventAsync([FromBody] CreateEventCommand command)
        => _mediator.Send(command);

    [HttpPut]
    public Task<EventDetailsDto> UpdateEventAsync([FromBody] UpdateEventCommand command)
        => _mediator.Send(command);

    [HttpDelete("{eventId}")]
    public Task<Unit> DeleteEventAsync(Guid eventId)
        => _mediator.Send(new DeleteEventCommand(eventId));

    #endregion

    #region Prices

    [HttpPost("prices")]
    public Task<EventDetailsDto> CreatePriceAsync([FromBody] CreatePriceCommand command)
        => _mediator.Send(command);

    #endregion

    #region Offers

    [HttpPost("offers")]
    public Task<EventDetailsDto> CreateOfferAsync([FromBody] CreateOfferCommand command)
        => _mediator.Send(command);

    #endregion
}