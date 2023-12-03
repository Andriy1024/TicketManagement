namespace TMS.Ticketing.Application.UseCases.VenueSeats;

public sealed class CreateSeatCommand : ICommand<VenueDetailsDto>, IValidatable
{
    public required Guid VenueId { get; init; }

    public required Guid SectionId { get; init; }

    public int? RowNumber { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.VenueId).NotEmpty();
            x.RuleFor(y => y.SectionId).NotEmpty();
        });
    }
}

internal sealed class CreateSeatHandlers : IRequestHandler<CreateSeatCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public CreateSeatHandlers(IVenuesRepository repository)
    {
        _repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        venue.CreateSeat(request.SectionId, request.RowNumber);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}