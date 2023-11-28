using MediatR;

using Microsoft.AspNetCore.Mvc;

using TMS.Common.IntegrationEvents;

namespace TMS.Ticketing.API.Controllers;

[Route("api/webhooks")]
[ApiController]
public class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebhooksController(IMediator mediator)
        => _mediator = mediator;

    // This webhook will be reimplemented as message brocker message in the message queues module
    [HttpPut("payments/status")]
    public Task PaymentStatusUpdatedAsync([FromBody] IntegrationEvent<PaymentStatusUpdated> @event)
        => _mediator.Send(@event);
}
