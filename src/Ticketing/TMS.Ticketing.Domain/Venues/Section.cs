namespace TMS.Ticketing.Domain.Venues;

public enum SectionType
{
    /// <summary>
    /// Specific designated place in a row.
    /// </summary>
    Designated = 1,

    /// <summary>
    /// Patrons can choose to sit anywhere within a particular section (for example, the dance floor)
    /// </summary>
    GeneralAdmission = 2
}

public sealed class Section
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; set; }

    public required SectionType Type { get; set; }

    public required List<Seat> Seats { get; init; } = new();
}