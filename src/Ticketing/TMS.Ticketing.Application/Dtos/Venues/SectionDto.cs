using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class SectionDto
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; set; }

    public required SectionType Type { get; set; }

    public required List<SeatDto> Seats { get; init; } = new();

    public static SectionDto Map(VenueSection section) => new SectionDto
    {
        SectionId = section.SectionId,
        VenueId = section.VenueId,
        Name = section.Name,
        Type = section.Type,
        Seats = section.Seats.Select(SeatDto.Map).ToList()
    };
}