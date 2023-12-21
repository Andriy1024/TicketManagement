using TMS.Common.Enums;
using TMS.Common.Errors;
using TMS.Common.IntegrationEvents.Notifications;
using TMS.Common.Interfaces;

using TMS.Notifications.Domain.Enums;

namespace TMS.Notifications.Domain.Models;

public class NotificationEntity : IEntity<Guid>
{
    public required Guid Id { get; init; }

    public required int AccountId { get; init; }

    public required NotificationStatus Status { get; set; }

    public required NotificationType Type { get; init; }

    public required NotificationPayload Payload { get; init; }

    public int Version { get; private set; } = 1;

    public static NotificationEntity Create(NotificationPayload payload)
    {
        return new NotificationEntity
        {
            Id = Guid.NewGuid(),
            AccountId = payload.AccountId,
            Payload = payload,
            Status = NotificationStatus.Pending,
            Type = GetNotificationType(payload)
        };
    }

    private static NotificationType GetNotificationType(INotificationPayload payload)
    {
        return payload switch
        {
            OrderStatusUpdatedNotification order when order.Status == OrderStatus.Pending => NotificationType.OrderPending,
            OrderStatusUpdatedNotification order when order.Status == OrderStatus.Completed => NotificationType.OrderCompleted,
            OrderStatusUpdatedNotification order when order.Status == OrderStatus.Cancelled => NotificationType.OrderCancelled,
            OrderStatusUpdatedNotification order when order.Status == OrderStatus.Failed => NotificationType.OrderFailed,
            _ => throw ApiError
                .InternalServerError($"Unexpected notification payload: {payload.GetType().Name}")
                .ToException()
        };
    }

    public (int Old, int New) IncreaseVersion() => (Version, ++Version);
}