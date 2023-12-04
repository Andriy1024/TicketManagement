using TMS.Ticketing.Application.UseCases.Offers;

namespace TMS.Ticketing.Applications.Test.Validation.Data;

public class OfferRequestData : RequestTestGenerator
{
    public OfferRequestData()
    {
        #region CreateOfferCommand

        ValidCase(new CreateOfferCommand
        {
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new CreateOfferCommand
        {
            EventId = Guid.Empty,
            SeatId = Guid.NewGuid(),
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new CreateOfferCommand
        {
            EventId = Guid.NewGuid(),
            SeatId = Guid.Empty,
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new CreateOfferCommand
        {
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            PriceId = Guid.Empty
        });

        #endregion
    }
}