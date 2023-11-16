namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class GetVenuesOverview : IRequest<IEnumerable<VenueOverviewDto>>
{
}

public sealed class GetVenueDetails : IRequest<VenueDetailsDto>
{
    public Guid Id { get; set; }
}