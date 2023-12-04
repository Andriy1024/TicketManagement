using MediatR;
using TMS.Common.Errors;
using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Domain.Events;

public sealed class EventEntity : IEntity<Guid>
{
    public required Guid Id { get; init; }
    
    public required string Name { get; set; }

    public List<Detail>? Details { get; set; }

    public required DateTime Start { get; set; }
    
    public required DateTime End { get; set; }

    public required int CreatorId { get; set; }

    public List<EventSeat> Seats { get; set; } = new();

    public List<Price> Prices { get; set; } = new();

    public List<Offer> Offers { get; set; } = new();

    public EventSeat GetSeat(Guid seatId) 
        => Seats.Find(x => x.SeatId == seatId)
            ?? throw ApiError
                .InvalidData($"Seat not found: {seatId}")
                .ToException();

    public EventEntity AddPrice(string name, decimal amount) 
    {
        Prices.Add(new Price
        {
            Id = Guid.NewGuid(),
            EventId = Id,
            Name = name,
            Amount = amount
        });

        return this;
    }
}