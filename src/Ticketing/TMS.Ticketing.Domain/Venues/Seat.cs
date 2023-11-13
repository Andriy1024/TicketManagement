namespace TMS.Ticketing.Domain.Venues;

public sealed class Seat
{
    public required Guid SectionId { get; init; }

    public required Guid SeatId { get; init; }

    public required int? SeatNumber { get; init; }

    public required int? RowNumber { get; init; }
}