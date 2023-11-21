using TMS.Payments.Domain.Views;

namespace TMS.Payments.Application.Interfaces;

public interface IPaymentsViewRepository
{
    Task<PaymentDetailsView?> GetPaymentDetailsAsync(Guid id, CancellationToken token);

    Task<UserPaymentsView?> GetUserPaymentsAsync(int accountId, CancellationToken token);
}