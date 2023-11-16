using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Handlers.Venues;

public sealed class VenuesHandler :
    IRequestHandler<DeleteVenueCommand, Unit>,
    IRequestHandler<CreateVenueCommand, VenueDetailsDto>,
    IRequestHandler<GetVenueDetails, VenueDetailsDto>,
    IRequestHandler<GetVenuesOverview, IEnumerable<VenueOverviewDto>>
{
    private readonly IVenuesRepository _repository;

    public VenuesHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = new VenueEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Country = request.Country,
            City = request.City,
            Street = request.Street,
            Details = request.Details
        };

        await _repository.AddAsync(venue);

        return VenueDetailsDto.Map(venue);
    }

    public async Task<Unit> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);

        return Unit.Value;
    }

    public async Task<VenueDetailsDto> Handle(GetVenueDetails request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.Id);

        return VenueDetailsDto.Map(venue);
    }

    public async Task<IEnumerable<VenueOverviewDto>> Handle(GetVenuesOverview request, CancellationToken cancellationToken)
    {
        var venues = await _repository.GetAllAsync(cancellationToken);

        return venues.Select(VenueOverviewDto.Map).ToList();
    }
}