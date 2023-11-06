using TMS.Notifications.Domain.Enums;
using TMS.Notifications.Domain.Interfaces;

namespace TMS.Notifications.Domain.Models;

public class Notification
{
    public required Guid Id { get; set; }

    public required int AccountId { get; set; }

    public required NotificationStatus Status { get; set; }

    public required NotificationType Type { get; set; }

    public required INotificationPayload Payload { get; set; }
}