namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class GetVenueDetails : IRequest<VenueDetailsDto>
{
    public Guid Id { get; set; }
}

public sealed class GetVenueDetailsHandler : IRequestHandler<GetVenueDetails, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public GetVenueDetailsHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(GetVenueDetails request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.Id);

        return VenueDetailsDto.Map(venue);
    }
}
