using TMS.Ticketing.Application.UseCases.Orders;

namespace TMS.Ticketing.Applications.Test.Validation.TestGenerators;

public class OrderRequestTestGenerator : RequestTestGenerator
{
    public OrderRequestTestGenerator()
    {
        #region CreateOrderCommand

        ValidCase(new CreateOrderCommand(Guid.NewGuid()));

        InvalidCase(new CreateOrderCommand(Guid.Empty));

        #endregion
    }
}