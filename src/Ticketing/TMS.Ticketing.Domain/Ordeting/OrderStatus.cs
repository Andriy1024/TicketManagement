namespace TMS.Ticketing.Domain.Ordering;

public enum OrderStatus
{
    PendingPayment = 1,
    Cancelled = 2,
    Failed = 3,
    Completed = 4,
    PendingRefund = 5,
    Refunded = 6
}
