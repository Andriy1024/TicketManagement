using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Infrastructure.Emails;

public class DisabledEmailsService : IEmailsService
{
    public Task SendAsync(NotificationEntity entity)
    {
        return Task.CompletedTask;
    }
}