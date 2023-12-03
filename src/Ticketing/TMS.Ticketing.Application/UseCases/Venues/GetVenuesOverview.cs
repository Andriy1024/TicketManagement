namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class GetVenuesOverview : IQuery<IEnumerable<VenueOverviewDto>>
{
}

internal sealed class GetVenuesOverviewHandler : IRequestHandler<GetVenuesOverview, IEnumerable<VenueOverviewDto>>
{
    private readonly IVenuesRepository _repository;

    public GetVenuesOverviewHandler(IVenuesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VenueOverviewDto>> Handle(GetVenuesOverview request, CancellationToken cancellationToken)
    {
        var venues = await _repository.GetAllAsync(cancellationToken);

        return venues.Select(VenueOverviewDto.Map).ToList();
    }
}