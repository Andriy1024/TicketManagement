using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Domain.Venues;

public sealed class Venue : IDocumentEntry<Guid>
{
    public static string Collection => "Venues";

    public required Guid Id { get; init; }
    
    public required string Name { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }

    public List<Detail>? Details { get; set; }

    public List<Section> Sections { get; set; } = new();
}