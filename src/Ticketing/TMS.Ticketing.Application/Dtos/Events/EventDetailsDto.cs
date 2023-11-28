using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class EventDetailsDto : EventOverviewDto
{
    public List<EventSeatDto> Seats { get; set; } = new();

    public List<OfferDto> Offers { get; set; } = new();

    public List<PriceDto> Prices { get; set; } = new();

    public static EventDetailsDto Map(EventEntity @event) => new()
    {
        Id = @event.Id,
        Name = @event.Name,
        Details = @event.Details,
        Start = @event.Start,
        End = @event.End,
        Seats = @event.Seats.Select(EventSeatDto.Map).ToList(),
        Offers = @event.Offers.Select(OfferDto.Map).ToList(),
        Prices = @event.Prices.Select(PriceDto.Map).ToList()
    };
}