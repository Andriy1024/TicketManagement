using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.Handlers.Venues;

public class VenueSeatHandlers :
    IRequestHandler<CreateSeatCommand, VenueDetailsDto>,
    IRequestHandler<DeleteSeatCommand, VenueDetailsDto>
{
    private readonly IVenuesRepository _repository;

    public VenueSeatHandlers(IVenuesRepository repository)
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