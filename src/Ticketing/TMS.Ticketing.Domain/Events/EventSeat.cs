namespace TMS.Ticketing.Domain.Events;

public sealed class EventSeat
{
    public required int EventId { get; set; }

    public required int SeatId { get; set; }

    public required SeatState State { get; set; }
}
