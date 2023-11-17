namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class GetVenuesOverview : IRequest<IEnumerable<VenueOverviewDto>>
{
}

public sealed class GetVenuesOverviewHandler : IRequestHandler<GetVenuesOverview, IEnumerable<VenueOverviewDto>>
{
    private readonly IVenuesRepository _repository;

    public GetVenuesOverviewHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEnumerable<VenueOverviewDto>> Handle(GetVenuesOverview request, CancellationToken cancellationToken)
    {
        var venues = await _repository.GetAllAsync(cancellationToken);

        return venues.Select(VenueOverviewDto.Map).ToList();
    }
}