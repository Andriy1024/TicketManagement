using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.UseCases.VenueSeats;

public sealed class CreateSeatCommand : IRequest<VenueDetailsDto>, IValidatable
{
    public required Guid VenueId { get; init; }

    public required Guid SectionId { get; init; }

    public required int? RowNumber { get; init; }

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
        this._repository = repository;
    }

    public async Task<VenueDetailsDto> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.VenueId);

        var section = venue.GetSection(request.SectionId);

        var rowSeats = section.Seats
            .Where(x => x.RowNumber == request.RowNumber)
            .ToArray();

        var newSeatNumber = rowSeats.Length == 0 ? 1
            : section.Seats[rowSeats.Length - 1].SeatNumber + 1;

        var seat = new VenueSeat
        {
            SeatId = Guid.NewGuid(),
            SectionId = section.SectionId,
            RowNumber = request.RowNumber,
            SeatNumber = newSeatNumber
        };

        section.Seats.Add(seat);

        await _repository.UpdateAsync(venue);

        return VenueDetailsDto.Map(venue);
    }
}