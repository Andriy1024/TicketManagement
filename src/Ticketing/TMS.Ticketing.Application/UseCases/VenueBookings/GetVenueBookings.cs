﻿using TMS.Ticketing.Application.Helpers;

namespace TMS.Ticketing.Application.UseCases.VenueBookings;

public sealed record GetVenueBookings(Guid VenueId) : IQuery<IEnumerable<VenueBookingDto>>, IValidatable, ICachable
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.VenueId).NotEmpty());
    }

    public string GetCacheKey() => CacheKeys.GetVenueBookingKey(VenueId);
}

internal sealed class GetVenueBookingsHandler : IRequestHandler<GetVenueBookings, IEnumerable<VenueBookingDto>>
{
    private readonly IVenuesBookingRepository _bookingRepo;

    public GetVenueBookingsHandler(IVenuesBookingRepository bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    public async Task<IEnumerable<VenueBookingDto>> Handle(GetVenueBookings request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepo.FindAsync(x => x.VenueId == request.VenueId);

        return booking.Select(VenueBookingDto.Map).ToList();
    }
}