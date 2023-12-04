using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class CreateVenueCommand : IRequest<VenueDetailsDto>, IValidatable
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public List<Detail>? Details { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.Name).NotEmpty());
    }
}

internal sealed class CreateVenueHandler : IRequestHandler<CreateVenueCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public CreateVenueHandler(IVenuesRepository repository)
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
}