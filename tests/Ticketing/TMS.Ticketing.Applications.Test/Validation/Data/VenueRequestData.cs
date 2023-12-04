using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Application.UseCases.VenueSeats;
using TMS.Ticketing.Application.UseCases.VenueSections;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Applications.Test.Validation.Data;

public class VenueRequestData : RequestTestGenerator
{
    public VenueRequestData()
    {
        #region CreateVenueCommand

        ValidCase(new CreateVenueCommand()
        {
            Name = "Venue 1"
        });

        InvalidCase(new CreateVenueCommand()
        {
            Name = ""
        });

        #endregion

        #region DeleteVenueCommand

        ValidCase(new DeleteVenueCommand(Guid.NewGuid()));

        InvalidCase(new DeleteVenueCommand(Guid.Empty));

        #endregion

        #region GetVenueDetails

        ValidCase(new GetVenueDetails(Guid.NewGuid()));

        InvalidCase(new GetVenueDetails(Guid.Empty));

        #endregion

        #region CreateSeatCommand

        ValidCase(new CreateSeatCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.NewGuid()
        });

        InvalidCase(new CreateSeatCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.Empty
        });

        InvalidCase(new CreateSeatCommand()
        {
            VenueId = Guid.Empty,
            SectionId = Guid.NewGuid()
        });

        #endregion

        #region DeleteSeatCommand

        ValidCase(new DeleteSeatCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteSeatCommand()
        {
            VenueId = Guid.Empty,
            SectionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteSeatCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.Empty,
            SeatId = Guid.NewGuid()
        });

        InvalidCase(new DeleteSeatCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            SeatId = Guid.Empty
        });

        #endregion

        #region CreateSectionCommand

        ValidCase(new CreateSectionCommand()
        {
            VenueId = Guid.NewGuid(),
            Name = "#1",
            Type = SectionType.Designated
        });

        InvalidCase(new CreateSectionCommand()
        {
            VenueId = Guid.Empty,
            Name = "#1",
            Type = SectionType.Designated
        });

        InvalidCase(new CreateSectionCommand()
        {
            VenueId = Guid.NewGuid(),
            Name = "",
            Type = SectionType.Designated
        });

        InvalidCase(new CreateSectionCommand()
        {
            VenueId = Guid.NewGuid(),
            Name = "#1",
            Type = (SectionType)133
        });

        #endregion

        #region DeleteSectionCommand

        ValidCase(new DeleteSectionCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.NewGuid()
        });

        InvalidCase(new DeleteSectionCommand()
        {
            VenueId = Guid.Empty,
            SectionId = Guid.NewGuid()
        });

        InvalidCase(new DeleteSectionCommand()
        {
            VenueId = Guid.NewGuid(),
            SectionId = Guid.Empty
        });

        #endregion

        #region UpdateSectionCommand

        ValidCase(new UpdateSectionCommand()
        {
            SectionId = Guid.NewGuid(),
            VenueId = Guid.NewGuid(),
            Name = "#1",
            Type = SectionType.GeneralAdmission
        });

        InvalidCase(new UpdateSectionCommand()
        {
            SectionId = Guid.Empty,
            VenueId = Guid.NewGuid(),
            Name = "#1",
            Type = SectionType.GeneralAdmission
        });

        InvalidCase(new UpdateSectionCommand()
        {
            SectionId = Guid.NewGuid(),
            VenueId = Guid.Empty,
            Name = "#1",
            Type = SectionType.GeneralAdmission
        });

        InvalidCase(new UpdateSectionCommand()
        {
            SectionId = Guid.NewGuid(),
            VenueId = Guid.NewGuid(),
            Name = "",
            Type = SectionType.GeneralAdmission
        });

        InvalidCase(new UpdateSectionCommand()
        {
            SectionId = Guid.NewGuid(),
            VenueId = Guid.NewGuid(),
            Name = "#1",
            Type = (SectionType)321
        });

        #endregion
    }
}