using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Application.UseCases;

public sealed record GetPaymentDetails(Guid PaymentId) : IRequest<PaymentDetailsView>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.PaymentId).NotEmpty());
    }
};

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
            throw ApiError.NotFound("Payment not found").ToException();
        }

        return payment;
    }
}
