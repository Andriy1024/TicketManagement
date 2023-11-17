using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class EventOverviewDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<KeyValePair>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public static EventOverviewDto Map(EventEntity @event) => new();
}

public class EventDetailsDto : EventOverviewDto
{
    public List<EventSeatDto> Seats { get; set; } = new();

    public List<OfferDto> Offers { get; set; } = new();

    public static EventDetailsDto Map(EventEntity @event) => new();
}

public class EventSeatDto
{
    public static EventSeatDto Map(EventSeat eventSeat) => new();
}

public class OfferDto
{
}
