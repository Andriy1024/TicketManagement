namespace TMS.Ticketing.Application.Services.Payments;

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
