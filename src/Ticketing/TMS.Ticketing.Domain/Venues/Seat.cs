namespace TMS.Ticketing.Domain.Venues;

public sealed class Seat
{
    public required int SeatId { get; init; }

    public required int RowId { get; init; }

    public required int SeatNumber { get; init; }
}