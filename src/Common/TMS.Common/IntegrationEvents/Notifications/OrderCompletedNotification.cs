using TMS.Common.Enums;

namespace TMS.Common.IntegrationEvents.Notifications;

public class NotificationPayload : INotificationPayload
{
    public int AccountId { get; set; }
}

public class OrderStatusUpdatedNotification : NotificationPayload
{
    public Guid OrderId { get; set; }

    public Guid EventId { get; set; }

    public OrderStatus Status { get; set; }

    public decimal Total { get; set; }

    public DateTime UpdatedAt { get; set; }
}