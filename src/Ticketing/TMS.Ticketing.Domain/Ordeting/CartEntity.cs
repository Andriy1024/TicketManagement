﻿using TMS.Common.Errors;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class CartEntity : Entity, IEntity<Guid>
{
    public required Guid Id { get; set; }

    public required int AccountId { get; init; }

    public decimal Total => this.OrderItems.Sum(x => x.Amount);

    public List<OrderItem> OrderItems { get; set; } = new();

    public int Version { get; set; } = 1;

    public CartEntity AddItem(
        EventEntity @event,
        Guid seatId,
        Guid priceId) 
    {
        var seat = @event.GetSeat(seatId);
        var price = @event.GetPrice(priceId);
        _ = @event.GetOffer(seatId, priceId);

        if (seat.State != SeatState.Available)
            throw ApiError.InvalidData("Seat is not available").ToException();

        var orderItem = new OrderItem
        {
            EventId = @event.Id,
            SeatId = seat.SeatId,
            PriceId = price.Id,
            Amount = price.Amount
        };

        OrderItems.Add(orderItem);
        
        return this;
    }

    public (int Old, int New) IncreaseVersion() => (Version, ++Version);
}