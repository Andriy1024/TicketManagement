using MediatR;

using TMS.Common.Interfaces;
using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Application.UseCases;

public record ProccessNotification(NotificationEntity Notification) : ICommand<Unit>;

internal sealed class ProccessNotificationHandler : IRequestHandler<ProccessNotification, Unit>
{
    public async Task<Unit> Handle(ProccessNotification request, CancellationToken cancellationToken)
    {
        // TODO: send email
        return Unit.Value;
    }
}