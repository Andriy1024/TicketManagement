using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.IntegrationTest.Common.FakeObjects;

public class FakeVenueFactory
{
    public static VenueEntity Create(Guid? venueId = null, string? name = null)
    {
        name ??= "Default Venue";
        venueId ??= Guid.NewGuid();
        var sectionId = Guid.NewGuid();

        return new VenueEntity
        {
            Id = venueId.Value,
            Name = name,
            City = "Denver",
            Country = "USA",
            Street = "Steet 45",
            Details = new List<Detail>()
            {
                new()
                {
                    Name = "Detail 1",
                    Value = "Detail Value"
                }
            },
            Sections = new List<VenueSection>()
            {
                new()
                {
                    SectionId = sectionId,
                    Name = "Section 1",
                    Type = SectionType.Designated,
                    VenueId = venueId.Value,
                    Seats = new List<VenueSeat>()
                    {
                        new VenueSeat()
                        {
                            SectionId = sectionId,
                            RowNumber = 1,
                            SeatId = Guid.NewGuid(),
                            SeatNumber = 1
                        },
                        new VenueSeat()
                        {
                            SectionId = sectionId,
                            RowNumber = 1,
                            SeatId = Guid.NewGuid(),
                            SeatNumber = 2
                        }
                    }
                }
            }
        };
    }
}