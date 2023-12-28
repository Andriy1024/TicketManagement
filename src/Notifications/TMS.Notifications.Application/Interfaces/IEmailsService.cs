using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Application.Interfaces;

public interface IEmailsService
{
    Task SendAsync(NotificationEntity entity);
}