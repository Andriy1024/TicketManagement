using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Domain.Views;

public class PaymentOverview
{
    public Guid PaymentId { get; set; }

    public int AccountId { get; set; }

    public PaymentStatus Status { get; set; }
}
