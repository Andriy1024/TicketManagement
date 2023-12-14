namespace TMS.Ticketing.Application.UseCases.VenueSeats;

public class DeleteSeatCommand : ICommand<VenueDetailsDto>, IValidatable
{
    public required Guid VenueId { get; init; }

    public required Guid SectionId { get; init; }

    public required Guid SeatId { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.VenueId).NotEmpty();
            x.RuleFor(y => y.SectionId).NotEmpty();
            x.RuleFor(y => y.SeatId).NotEmpty();
        });
    }
}

internal sealed class DeleteSeatHandler : IRequestHandler<DeleteSeatCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public DeleteSeatHandler(IVenuesRepository repository)
    {
        _repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        venue.DeleteSeat(request.SectionId, request.SeatId);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}