namespace TMS.Ticketing.Domain.Events;

public sealed class EventSeat
{
    public required Guid SeatId { get; init; }

    public required SeatState State { get; set; }
}