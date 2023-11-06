namespace TMS.Ticketing.Domain.Venues;

public sealed class Section
{
    public required int SectionId { get; init; }

    public required int VenueId { get; init; }

    public required string Name { get; init; }

    public required SectionType Type { get; init; }

    public required List<SectionRow> Rows { get; init; } = new();
}