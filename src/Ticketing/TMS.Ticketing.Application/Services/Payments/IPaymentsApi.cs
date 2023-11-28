using Refit;

namespace TMS.Ticketing.Application.Services.Payments;

public interface IPaymentsApi
{
    [Post("/api/payments")]
    Task CreatePaymentAsync([Body] CreatePaymentRequest request);
}