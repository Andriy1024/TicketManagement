using TMS.Common.IntegrationEvents.Notifications;

using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Domain.DomainEvents;
using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.DomainEvents;

internal sealed class OrderStatusUpdatedHandler : 
    INotificationHandler<OrderStatusUpdated>,
    INotificationHandler<EntityCreated<OrderEntity>>
{
    private readonly ITicketingMessageBrocker _messageBrocker;

    public OrderStatusUpdatedHandler(ITicketingMessageBrocker messageBrocker)
    {
        _messageBrocker = messageBrocker;
    }

    public Task Handle(OrderStatusUpdated notification, CancellationToken cancellationToken)
        => SendIntegrationEvent(notification.Order);
    
    public Task Handle(EntityCreated<OrderEntity> notification, CancellationToken cancellationToken)
        => SendIntegrationEvent(notification.Entity);
    
    private Task SendIntegrationEvent(OrderEntity order)
    {
        var notificationEntity = new OrderStatusUpdatedNotification()
        {
            OrderId = order.Id,
            EventId = order.EventId,
            Total = order.Total,
            UpdatedAt = order.UpdatedAt,
            AccountId = order.AccountId,
            Status = order.Status
        };

        _messageBrocker.Send(notificationEntity);

        return Task.CompletedTask;
    }
}