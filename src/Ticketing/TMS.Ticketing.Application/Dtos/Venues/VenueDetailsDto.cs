using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class VenueDetailsDto : VenueOverviewDto
{
    public required IEnumerable<SectionDto> Sections { get; init; }

    public static VenueDetailsDto Map(VenueEntity venue) => new()
    {
        Id = venue.Id,
        Name = venue.Name,
        Country = venue.Country,
        City = venue.City,
        Street = venue.Street,
        Details = venue.Details,
        Sections = venue.Sections
            .Select(SectionDto.Map)
            .ToList()
    };
}