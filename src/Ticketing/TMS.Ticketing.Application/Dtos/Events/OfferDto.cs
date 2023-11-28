using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class OfferDto
{
    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }

    public static OfferDto Map(Offer eventOffer) => new()
    {
        SeatId = eventOffer.SeatId,
        PriceId = eventOffer.PriceId
    };
}