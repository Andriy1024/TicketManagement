using TMS.Common.Interfaces;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;

namespace TMS.Ticketing.Persistence.StartupTask;

/// <summary>
/// The task is created for local testing purposes.
/// </summary>
internal class MongoSeedTask : IStartupTask
{
    private readonly IVenuesRepository venues;

    public MongoSeedTask(IVenuesRepository venues)
    {
        this.venues = venues;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var defaultVenuName = "Venue #1";

        var defaultVenu = await venues.GetAsync(x => x.Name == defaultVenuName, cancellationToken);

        if (defaultVenu == null)
        {
            var venueId = Guid.NewGuid();
            var sectionId = Guid.NewGuid();

            defaultVenu = new VenueEntity 
            {
                Id = venueId,
                Name = defaultVenuName,
                City = "Krakow",
                Country = "Poland",
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
                        VenueId = venueId,
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

            await venues.AddAsync(defaultVenu);
        }
    }
}