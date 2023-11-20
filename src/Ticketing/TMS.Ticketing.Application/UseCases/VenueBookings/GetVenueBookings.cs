namespace TMS.Ticketing.Application.UseCases.VenueBookings;

public sealed record GetVenueBookings(Guid VenueId) : IRequest<IEnumerable<VenueBookingDto>>;

internal sealed class GetVenueBookingsHandler : IRequestHandler<GetVenueBookings, IEnumerable<VenueBookingDto>>
{
    private readonly IVenuesBookingRepository _bookingRepo;

    public GetVenueBookingsHandler(IVenuesBookingRepository bookingRepo)
    {
        this._bookingRepo = bookingRepo;
    }

    public async Task<IEnumerable<VenueBookingDto>> Handle(GetVenueBookings request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepo.FindAsync(x => x.VenueId == request.VenueId);

        return booking.Select(VenueBookingDto.Map).ToList();
    }
}