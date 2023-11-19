using MediatR;

using Microsoft.AspNetCore.Mvc;

using TMS.Payments.Application.UseCases;

namespace TMS.Payments.API.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<CreatePaymentResult> CreatePaymentAsync([FromBody] CreatePaymentCommand command)
        => _mediator.Send(command);

    [HttpPut("{paymentId}/complete")]
    public Task<CompletePaymentResult> CompletePaymentAsync([FromRoute] Guid paymentId)
        => _mediator.Send(new CompletePaymentCommand(paymentId));

    [HttpPut("{paymentId}/fail")]
    public Task<CompletePaymentResult> FailPaymentAsync([FromRoute] Guid paymentId)
        => _mediator.Send(new FailPaymentCommand(paymentId));
}