using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class CreateVenueCommand : IRequest<VenueDetailsDto>
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public List<KeyValePair>? Details { get; set; }
}

public sealed class DeleteVenueCommand : IRequest<Unit>
{
    public required Guid Id { get; init; }
}