using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.IntegrationTest.Common.FakeObjects;

public static class FakeModelBuilders
{
    public static EventEntity GeneratePrices(this EventEntity @event)
    {
        return @event
            .AddPrice("Adult", 10)
            .AddPrice("Children", 5);
    }

    public static EventEntity GenerateOffers(this EventEntity @event)
    {
        var offers =
           from seat in @event.Seats
           from price in @event.Prices
           select new Offer
           {
               SeatId = seat.SeatId,
               PriceId = price.Id
           };

        @event.Offers = offers.ToList();

        return @event;
    }
}