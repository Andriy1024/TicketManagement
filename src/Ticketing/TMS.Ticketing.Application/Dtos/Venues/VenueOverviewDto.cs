using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class VenueOverviewDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public List<Detail>? Details { get; set; }

    public static VenueOverviewDto Map(VenueEntity venue) => new()
    {
        Id = venue.Id,
        Name = venue.Name,
        Country = venue.Country,
        City = venue.City,
        Street = venue.Street,
        Details = venue.Details
    };
}
