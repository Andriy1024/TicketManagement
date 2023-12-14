namespace TMS.Ticketing.Infrastructure.Payments.API;

public sealed class CreatePaymentRequest
{
    public Guid PaymentId { get; set; }

    // Will be taken from jwt token in future.
    public int AccountId { get; set; }

    public decimal Amount { get; set; }
}
