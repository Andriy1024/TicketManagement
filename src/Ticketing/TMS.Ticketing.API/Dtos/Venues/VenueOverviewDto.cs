using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.API.Dtos.Venues;

public class VenueOverviewDto : VenuePropertiesDto
{
    public Guid Id { get; set; }
}

public class VenuePropertiesDto
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public List<Detail>? Details { get; set; }
}