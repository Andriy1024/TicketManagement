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
}