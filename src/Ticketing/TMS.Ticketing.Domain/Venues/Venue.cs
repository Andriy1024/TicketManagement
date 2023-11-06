using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Domain.Venues;

public sealed class Venue
{
    public required int Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Country { get; init; }
    
    public required string City { get; init; }
    
    public required string Street { get; init; }

    public required List<Detail> Details { get; init; } = new();

    public required List<Section> Sections { get; init; } = new();

    public required List<VenueBooking> Bookings { get; init; } = new();
}