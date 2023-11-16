using TMS.Common.Errors;

using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Handlers.Venues;

internal sealed class VenueSectionsHandlers :
    IRequestHandler<CreateSectionCommand, VenueDetailsDto>,
    IRequestHandler<UpdateSectionCommand, VenueDetailsDto>,
    IRequestHandler<DeleteSectionCommand, VenueDetailsDto>
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

        await _repository.AddAsync(venue);

        return VenueDetailsDto.Map(venue);
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

    public async Task<VenueDetailsDto> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = venue.GetSection(request.SectionId);

        venue.Sections.Remove(section);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}