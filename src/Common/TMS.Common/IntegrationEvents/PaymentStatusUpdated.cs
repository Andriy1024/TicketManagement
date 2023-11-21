using TMS.Common.Enums;

namespace TMS.Common.IntegrationEvents;

public class PaymentStatusUpdated
{
    public Guid PaymentId { get; set; }

    public PaymentStatus Status { get; set; }
}