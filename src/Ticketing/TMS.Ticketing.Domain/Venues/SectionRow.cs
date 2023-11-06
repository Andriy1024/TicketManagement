namespace TMS.Ticketing.Domain.Venues;

public sealed class SectionRow
{
    public required int RowId { get; init; }

    public required int SectionId { get; init; }

    public required int RowNumber { get; init; }

    public required List<Seat> Seats { get; init; } = new();
}