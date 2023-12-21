using TMS.Common.Enums;
using TMS.Common.Errors;
using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Ticketing.Domain.DomainEvents;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class OrderEntity : EventDrivenEntity<Guid>
{
    public Guid EventId { get; init; }

    public int AccountId { get; init; }

    public OrderStatus Status { get; set; }

    public Guid PaymentId { get; set; }

    public decimal Total => this.OrderItems.Sum(x => x.Amount);

    public required OrderItem[] OrderItems { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }

    public static OrderEntity Create(
        CurrentUser user, 
        EventEntity @event,
        Guid paymentId,
        IEnumerable<OrderItem> orderItems)
    {
        if (orderItems.IsNullOrEmpty())
        {
            throw ApiError.InvalidData("Order items are empty")
                .ToException();
        }

        foreach (var orderItem in orderItems)
        {
            @event.BookSeat(orderItem.SeatId);
        }

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            AccountId = user.Id,
            EventId = @event.Id,
            OrderItems = orderItems.ToArray(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            PaymentId = paymentId
        };

        order.AddDomainEvent(new EntityCreated<OrderEntity>(order));

        return order;
    }

    public OrderEntity UpdateStatus(PaymentStatus payment)
    {
        var newOrderStatus = payment switch
        {
            PaymentStatus.Completed => OrderStatus.Completed,
            PaymentStatus.Failed => OrderStatus.Failed,
            _ => throw new NotImplementedException($"Unexpected Payment Status: {payment}")
        };

        return UpdateStatus(newOrderStatus);
    }

    public OrderEntity UpdateStatus(OrderStatus newStatus)
    {
        var error = newStatus switch
        {
            OrderStatus.Completed when Status == OrderStatus.Pending => null,
            OrderStatus.Failed when Status == OrderStatus.Pending => null,
            _ => ApiError.InvalidData($"Current order status: {Status} doesn't allow update to: {newStatus}")
        };

        if (error != null) throw error.ToException();

        Status = newStatus;

        AddDomainEvent(new OrderStatusUpdated(this));

        return this;
    }
}