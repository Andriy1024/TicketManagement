using TMS.Common.Errors;
using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Domain.Venues;

public sealed class VenueEntity : IEntity<Guid>
{
    public required Guid Id { get; init; }
    
    public required string Name { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }

    public List<Detail>? Details { get; set; }

    public List<VenueSection> Sections { get; set; } = new();

    public VenueSection GetSection(Guid sectionId)
    {
        return Sections.Find(x => x.SectionId == sectionId) 
            ?? throw ApiError.NotFound($"Section not found: {sectionId}").ToException();
    }
}