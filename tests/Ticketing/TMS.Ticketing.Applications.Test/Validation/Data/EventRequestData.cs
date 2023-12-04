using TMS.Ticketing.Application.UseCases.Events;

namespace TMS.Ticketing.Applications.Test.Validation.Data;

public class EventRequestData : RequestTestGenerator
{
    public EventRequestData()
    {
        #region CreateEventCommand

        ValidCase(new CreateEventCommand
        {
            Name = "#1",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new CreateEventCommand
        {
            Name = "",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new CreateEventCommand
        {
            Name = "#1",
            Start = default,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new CreateEventCommand
        {
            Name = "#1",
            Start = DateTime.UtcNow,
            End = default
        });

        InvalidCase(new CreateEventCommand
        {
            Name = "#1",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(-2)
        });

        #endregion

        #region DeleteEventCommand

        ValidCase(new DeleteEventCommand(Guid.NewGuid()));

        InvalidCase(new DeleteEventCommand(Guid.Empty));

        #endregion

        #region GetEventDetails

        ValidCase(new GetEventDetails(Guid.NewGuid()));

        InvalidCase(new GetEventDetails(Guid.Empty));

        #endregion

        #region UpdateEventCommand

        ValidCase(new UpdateEventCommand
        {
            EventId = Guid.NewGuid(),
            Name = "#1",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new UpdateEventCommand
        {
            EventId = Guid.Empty,
            Name = "#1",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new UpdateEventCommand
        {
            EventId = Guid.NewGuid(),
            Name = "",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new UpdateEventCommand
        {
            EventId = Guid.NewGuid(),
            Name = "#1",
            Start = default,
            End = DateTime.UtcNow.AddHours(2)
        });

        InvalidCase(new UpdateEventCommand
        {
            EventId = Guid.NewGuid(),
            Name = "#1",
            Start = DateTime.UtcNow,
            End = default
        });

        InvalidCase(new UpdateEventCommand
        {
            EventId = Guid.NewGuid(),
            Name = "#1",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(-2)
        });

        #endregion
    }
}