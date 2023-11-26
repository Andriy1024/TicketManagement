using TMS.Ticketing.Application.UseCases.VenueBookings;

namespace TMS.Ticketing.Applications.Test.Validation.TestGenerators;

public class VenueBookingRequestTestGenerator : RequestTestGenerator
{
    public VenueBookingRequestTestGenerator()
    {
        #region GetVenueBookings

        ValidCase(new GetVenueBookings(Guid.NewGuid()));

        InvalidCase(new GetVenueBookings(Guid.Empty));

        #endregion

        #region CreateVenueBookingCommand

        ValidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2),
        });

        InvalidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.Empty,
            EventId = Guid.NewGuid(),
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2),
        });

        InvalidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.NewGuid(),
            EventId = Guid.Empty,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(2),
        });

        InvalidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Start = default,
            End = DateTime.UtcNow.AddHours(2),
        });

        InvalidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Start = DateTime.UtcNow,
            End = default
        });

        InvalidCase(new CreateVenueBookingCommand
        {
            VenueId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(-1)
        });

        #endregion
    }
}