using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.VenueSections;

public sealed class UpdateSectionCommand : IRequest<VenueDetailsDto>, IValidatable
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public required string Name { get; init; }

    public required SectionType Type { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.SectionId).NotEmpty();
            x.RuleFor(y => y.VenueId).NotEmpty();
            x.RuleFor(y => y.Name).NotEmpty();
            x.RuleFor(y => y.Type).IsInEnum();
        });
    }
}

internal sealed class UpdateSectionHandler : IRequestHandler<UpdateSectionCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public UpdateSectionHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = venue.GetSection(request.SectionId);

        section.Name = request.Name;
        section.Type = request.Type;

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}