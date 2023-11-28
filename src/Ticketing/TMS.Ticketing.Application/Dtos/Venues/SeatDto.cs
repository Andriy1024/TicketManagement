using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Dtos;

public class SeatDto
{
    public required Guid SeatId { get; init; }

    public required int? SeatNumber { get; init; }

    public required int? RowNumber { get; init; }

    public static SeatDto Map(VenueSeat seat) => new()
    {
        SeatId = seat.SeatId,
        SeatNumber = seat.SeatNumber,
        RowNumber = seat.RowNumber
    };
}