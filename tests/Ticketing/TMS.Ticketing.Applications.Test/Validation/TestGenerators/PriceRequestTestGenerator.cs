using TMS.Ticketing.Application.UseCases.Prices;

namespace TMS.Ticketing.Applications.Test.Validation.TestGenerators;

public class PriceRequestTestGenerator : RequestTestGenerator
{
    public PriceRequestTestGenerator()
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