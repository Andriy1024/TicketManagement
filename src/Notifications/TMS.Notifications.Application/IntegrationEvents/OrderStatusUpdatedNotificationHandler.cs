using MediatR;

using TMS.Common.IntegrationEvents.Notifications;
using TMS.Common.IntegrationEvents;

using TMS.Notifications.Domain.Models;
using TMS.Notifications.Application.Interfaces;

namespace TMS.Notifications.Application.IntegrationEvents;

public class OrderStatusUpdatedNotificationHandler : IRequestHandler<IntegrationEvent<OrderStatusUpdatedNotification>, Unit>
{
    private readonly INotificationsRepository _repository;

    public OrderStatusUpdatedNotificationHandler(INotificationsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(IntegrationEvent<OrderStatusUpdatedNotification> request, CancellationToken cancellationToken)
    {
        var notification = NotificationEntity.Create(request.Payload);

        await _repository.AddAsync(notification, CancellationToken.None);

        return Unit.Value;
    }
}
