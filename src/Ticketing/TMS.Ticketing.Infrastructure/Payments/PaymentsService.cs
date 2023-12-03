using TMS.Ticketing.Application.Interfaces;

using TMS.Ticketing.Infrastructure.Payments.API;

namespace TMS.Ticketing.Infrastructure.Payments;

public sealed class PaymentsService : IPaymentsService
{
    private readonly IPaymentsApi _paymentsApi;

    public PaymentsService(IPaymentsApi paymentsApi)
    {
        this._paymentsApi = paymentsApi;
    }

    public Task CreatePaymentAsync(Guid id, decimal amount, int accointId)
    {
        return _paymentsApi.CreatePaymentAsync(new CreatePaymentRequest 
        {
            PaymentId = id,
            Amount = amount,
            AccountId = accointId
        });
    }
}
