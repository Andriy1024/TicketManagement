using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class CreateSectionCommand : IRequest<VenueDetailsDto>
{
    public required Guid VenueId { get; init; }

    public required string Name { get; init; }

    public required SectionType Type { get; init; }
}

public sealed class DeleteSectionCommand : IRequest<VenueDetailsDto>
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }
}

public sealed class UpdateSectionCommand : IRequest<VenueDetailsDto>
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; init; }

    public required SectionType Type { get; init; }
}