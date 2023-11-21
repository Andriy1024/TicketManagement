using MediatR;

using TMS.Common.Errors;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Application.UseCases;

public sealed record GetPaymentDetails(Guid PaymentId) : IRequest<PaymentDetailsView>;

internal sealed class GetPaymentDetailsHandler : IRequestHandler<GetPaymentDetails, PaymentDetailsView>
{
    private readonly IPaymentsViewRepository _repository;

    public GetPaymentDetailsHandler(IPaymentsViewRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentDetailsView> Handle(GetPaymentDetails request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetPaymentDetailsAsync(request.PaymentId, cancellationToken);

        if (payment == null)
        {
            throw AppError.NotFound("Payment not found").ToException();
        }

        return payment;
    }
}
