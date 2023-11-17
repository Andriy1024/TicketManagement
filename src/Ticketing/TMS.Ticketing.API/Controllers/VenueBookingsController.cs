using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.UseCases.VenueBookings;

namespace TMS.Ticketing.API.Controllers;

[Route("api/venues")]
[ApiController]
public class VenueBookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VenueBookingsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("{venueId}/bookings")]
    public Task GetVenueBookingsAsync([FromRoute] Guid venueId)
        => _mediator.Send(new GetVenueBookings { VenueId = venueId });

    [HttpPost("book")]
    public Task<VenueBookingDto> CreateVenueBookingAsync([FromBody] CreateVenueBookingCommand command)
        => _mediator.Send(command);
}