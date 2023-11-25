using TMS.Common.Errors;

namespace TMS.Ticketing.Domain.Venues;

public sealed class VenueSection
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; set; }

    public required SectionType Type { get; set; }

    public List<VenueSeat> Seats { get; set; } = new();

    public VenueSeat GetSeat(Guid seatId) 
    {
        return Seats.Find(x => x.SeatId == seatId)
            ?? throw ApiError.NotFound($"Seat not found: {seatId}").ToException();
    }
}