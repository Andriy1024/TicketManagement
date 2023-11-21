﻿using TMS.Common.Errors;
using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordeting;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class OrderEntity : IEntity<Guid>
{
    public Guid Id { get; init; }

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
            throw AppError.InvalidData("Order items are empty")
                .ToException();
        }

        foreach (var orderItem in orderItems)
        {
            var eventSeat = @event.GetSeat(orderItem.SeatId);

            if (eventSeat.State != SeatState.Available)
            {
                throw AppError.InvalidData("Seat is not available").ToException();
            }

            eventSeat.State = SeatState.Booked;
        }

        return new OrderEntity
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
    }
}