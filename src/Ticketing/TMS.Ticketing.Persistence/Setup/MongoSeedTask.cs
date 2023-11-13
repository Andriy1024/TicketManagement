using TMS.Common.Interfaces;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;

namespace TMS.Ticketing.Persistence.Setup;

/// <summary>
/// The task is created for local testing purposes.
/// </summary>
internal class MongoSeedTask : IStartupTask
{
    private readonly IRepository<VenueEntity, Guid> venues;

    public MongoSeedTask(IRepository<VenueEntity, Guid> venues)
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
                Details = new List<KeyValePair>() 
                {
                    new()
                    {
                        Name = "Detail 1",
                        Value = "Detail Value"
                    }
                },
                Sections = new List<Section>() 
                {
                    new() 
                    {
                        SectionId = sectionId,
                        Name = "Section 1",
                        Type = SectionType.Designated,
                        VenueId = venueId,
                        Seats = new List<Seat>() 
                        {
                            new Seat()
                            {
                                SectionId = sectionId,
                                RowNumber = 1,
                                SeatId = Guid.NewGuid(),
                                SeatNumber = 1
                            },
                            new Seat()
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