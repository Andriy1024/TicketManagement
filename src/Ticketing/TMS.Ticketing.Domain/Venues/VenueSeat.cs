namespace TMS.Ticketing.Domain.Venues;

public sealed class VenueSeat
{
    public required Guid SectionId { get; init; }

    public required Guid SeatId { get; init; }

    public required int? SeatNumber { get; set; }

    public required int? RowNumber { get; init; }
}