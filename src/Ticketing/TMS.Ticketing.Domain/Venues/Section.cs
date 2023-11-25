namespace TMS.Ticketing.Domain.Venues;

public sealed class Section
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; set; }

    public required SectionType Type { get; set; }

    public required List<Seat> Seats { get; init; } = new();
}