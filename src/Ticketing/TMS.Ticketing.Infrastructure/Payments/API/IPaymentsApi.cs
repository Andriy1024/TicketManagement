using Refit;

namespace TMS.Ticketing.Infrastructure.Payments.API;

public interface IPaymentsApi
{
    [Post("/api/payments")]
    Task CreatePaymentAsync([Body] CreatePaymentRequest request);
}