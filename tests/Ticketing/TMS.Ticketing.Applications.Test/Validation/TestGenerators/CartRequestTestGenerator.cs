using TMS.Ticketing.Application.UseCases.Carts;

namespace TMS.Ticketing.Applications.Test.Validation.TestGenerators;

public class CartRequestTestGenerator : RequestTestGenerator
{
    public CartRequestTestGenerator()
    {
        #region AddItemToCartCommand

        ValidCase(new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new AddItemToCartCommand
        {
            CartId = Guid.Empty,
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.Empty,
            SeatId = Guid.NewGuid(),
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SeatId = Guid.Empty,
            PriceId = Guid.NewGuid()
        });

        InvalidCase(new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            PriceId = Guid.Empty
        });

        #endregion

        #region DeleteItemFromCartCommand

        ValidCase(new DeleteItemFromCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteItemFromCartCommand
        {
            CartId = Guid.Empty,
            EventId = Guid.NewGuid(),
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteItemFromCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.Empty,
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteItemFromCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            SeatId = Guid.Empty
        });

        #endregion

        #region GetCartDetails

        ValidCase(new GetCartDetails(Guid.NewGuid()));

        InvalidCase(new GetCartDetails(Guid.Empty));

        #endregion
    }
}