namespace TMS.Ticketing.Application.UseCases.VenueSections;

public sealed class DeleteSectionCommand : IRequest<VenueDetailsDto>, IValidatable
{
    public required Guid SectionId { get; init; }

    public required Guid VenueId { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.SectionId).NotEmpty();
            x.RuleFor(y => y.VenueId).NotEmpty();
        });
    }
}

internal sealed class DeleteSectionHandler : IRequestHandler<DeleteSectionCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public DeleteSectionHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = venue.GetSection(request.SectionId);
        
        venue.Sections.Remove(section);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}