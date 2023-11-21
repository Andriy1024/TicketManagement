using Marten;

using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Persistence.Implementations;

public sealed class PaymentsViewRepository : IPaymentsViewRepository
{
    private readonly IDocumentSession _session;

    public PaymentsViewRepository(IDocumentSession session)
    {
        _session = session;
    }

    public Task<PaymentDetailsView?> GetPaymentDetailsAsync(Guid id, CancellationToken token)
    {
        return _session
            .Query<PaymentDetailsView>()
            .FirstOrDefaultAsync(c => c.PaymentId == id, token);
    }

    public Task<UserPaymentsView?> GetUserPaymentsAsync(int accountId, CancellationToken token)
    {
        return _session
            .Query<UserPaymentsView>()
            .FirstOrDefaultAsync(c => c.AccountId == accountId, token);
    }
}
