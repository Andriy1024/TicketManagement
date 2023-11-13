using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Domain.Venues;

public sealed class VenueEntity : IEntity<Guid>
{
    public required Guid Id { get; init; }
    
    public required string Name { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }

    public List<KeyValePair>? Details { get; set; }

    public List<Section> Sections { get; set; } = new();
}