using TMS.Ticketing.Application.UseCases.Orders;

namespace TMS.Ticketing.Applications.Test.Validation.Data;

public class OrderRequestData : RequestTestGenerator
{
    public OrderRequestData()
    {
        #region CreateOrderCommand

        ValidCase(new CreateOrderCommand(Guid.NewGuid()));

        InvalidCase(new CreateOrderCommand(Guid.Empty));

        #endregion
    }
}