using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class EventSeatDto
{
    public Guid SeatId { get; set; }

    public SeatState State { get; set; }

    public static EventSeatDto Map(EventSeat eventSeat) => new()
    {
        SeatId = eventSeat.SeatId,
        State = eventSeat.State
    };
}
