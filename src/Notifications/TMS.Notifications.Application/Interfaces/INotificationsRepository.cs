using TMS.Common.Interfaces;

using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Application.Interfaces;

public interface INotificationsRepository : IRepository<NotificationEntity, Guid>
{
}