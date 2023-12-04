using TMS.Ticketing.Application.UseCases.Prices;

namespace TMS.Ticketing.Applications.Test.Validation.Data;

public class PriceRequestData : RequestTestGenerator
{
    public PriceRequestData()
    {
        #region CreatePriceCommand

        ValidCase(new CreatePriceCommand
        {
            EventId = Guid.NewGuid(),
            Amount = 10,
            Name = "#1",
        });

        InvalidCase(new CreatePriceCommand
        {
            EventId = Guid.Empty,
            Amount = 10,
            Name = "#1",
        });

        InvalidCase(new CreatePriceCommand
        {
            EventId = Guid.NewGuid(),
            Amount = 0,
            Name = "#1",
        });

        InvalidCase(new CreatePriceCommand
        {
            EventId = Guid.NewGuid(),
            Amount = -1,
            Name = "#1",
        });

        InvalidCase(new CreatePriceCommand
        {
            EventId = Guid.NewGuid(),
            Amount = 10,
            Name = "",
        });

        #endregion
    }
}