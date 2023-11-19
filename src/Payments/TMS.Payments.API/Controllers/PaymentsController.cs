using MediatR;

using Microsoft.AspNetCore.Mvc;

using TMS.Payments.Application.UseCases;
using TMS.Payments.Domain.Views;

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

    [HttpGet("my")]
    public Task<UserPaymentsView> GetUserPaymentsAsync(CancellationToken token)
        => _mediator.Send(new GetUserPayments(), token);

    [HttpGet("{paymentId}")]
    public Task<PaymentDetailsView> GetPaymentDetailsAsync([FromRoute] Guid paymentId, CancellationToken token)
        => _mediator.Send(new GetPaymentDetails(paymentId), token);

    [HttpPost]
    public Task<CreatePaymentResult> CreatePaymentAsync([FromBody] CreatePaymentCommand command)
        => _mediator.Send(command);

    [HttpPut("{paymentId}/complete")]
    public Task<CompletePaymentResult> CompletePaymentAsync([FromRoute] Guid paymentId)
        => _mediator.Send(new CompletePaymentCommand(paymentId));

    [HttpPut("{paymentId}/fail")]
    public Task<FailPaymentResult> FailPaymentAsync([FromRoute] Guid paymentId)
        => _mediator.Send(new FailPaymentCommand(paymentId));
}