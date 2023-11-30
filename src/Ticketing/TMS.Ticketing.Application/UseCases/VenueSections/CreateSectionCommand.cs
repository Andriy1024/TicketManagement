using TMS.Ticketing.Application.Cache;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.VenueSections;

public sealed class CreateSectionCommand 
    : ICommand<VenueDetailsDto>, IValidatable, ICachable
{
    public required Guid VenueId { get; init; }

    public required string Name { get; init; }

    public required SectionType Type { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.VenueId).NotEmpty();
            x.RuleFor(y => y.Name).NotEmpty();
            x.RuleFor(y => y.Type).IsInEnum();
        });
    }

    public string GetCacheKey() => VenueCacheKey.GetKey(VenueId);
}

internal sealed class VenueSectionsHandlers : IRequestHandler<CreateSectionCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public VenueSectionsHandlers(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = new VenueSection
        {
            SectionId = Guid.NewGuid(),
            VenueId = venue.Id,
            Name = request.Name,
            Type = request.Type,
        };

        venue.Sections.Add(section);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}