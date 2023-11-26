namespace TMS.Ticketing.Application.UseCases.VenueSeats;

public class DeleteSeatCommand : IRequest<VenueDetailsDto>, IValidatable
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

public class DeleteSeatHandler : IRequestHandler<DeleteSeatCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public DeleteSeatHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = venue.GetSection(request.SectionId);

        var seat = section.GetSeat(request.SeatId);

        section.Seats.Remove(seat);

        var rowSeat = section.Seats
            .Where(x => x.RowNumber == seat.RowNumber)
            .ToArray();

        for (int i = 0; i < rowSeat.Length; i++)
        {
            rowSeat[i].SeatNumber = i + 1;
        }

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}