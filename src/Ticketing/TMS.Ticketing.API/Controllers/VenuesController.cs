using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.UseCases.VenueSections;
using TMS.Ticketing.Application.UseCases.VenueSeats;

using Microsoft.AspNetCore.Mvc;

using MediatR;

namespace TMS.Ticketing.API.Controllers;

[Route("api/venues")]
[ApiController]
public sealed class VenuesController : ControllerBase
{
    #region Context

    private readonly IMediator _mediator;

    public VenuesController(IMediator mediator)
        => _mediator = mediator;

    #endregion

    #region Venues

    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    [HttpGet("overview")]
    public Task<IEnumerable<VenueOverviewDto>> GetVenuesOverviewAsync(CancellationToken token)
        => _mediator.Send(new GetVenuesOverview(), token);

    [HttpGet("{venueId}")]
    public Task<VenueDetailsDto> GetVenueDetailsAsync([FromRoute] Guid venueId, CancellationToken token)
        => _mediator.Send(new GetVenueDetails(venueId), token);

    [HttpPost]
    public Task<VenueDetailsDto> CreateVenueAsync([FromBody] CreateVenueCommand command)
        => _mediator.Send(command);

    [HttpDelete("{venueId}")]
    public Task<Unit> DeleteVenueAsync(Guid venueId)
        => _mediator.Send(new DeleteVenueCommand(venueId));

    #endregion

    #region Sections

    [HttpPost("sections")]
    public Task<VenueDetailsDto> CreateSectionAsync(CreateSectionCommand command)
        => _mediator.Send(command);

    [HttpPut("sections")]
    public Task<VenueDetailsDto> UpdateSectionAsync(UpdateSectionCommand command)
        => _mediator.Send(command);

    [HttpDelete("{venueId}/sections/{sectionId}")]
    public Task<VenueDetailsDto> GetSectionsAsync(Guid venueId, Guid sectionId)
        => _mediator.Send(new DeleteSectionCommand 
            { 
                VenueId = venueId, 
                SectionId = sectionId 
            });

    #endregion

    #region Seats

    [HttpPost("sections/seats")]
    public Task<VenueDetailsDto> CreateSeatAsync(CreateSeatCommand command)
        => _mediator.Send(command);

    [HttpDelete("{venueId}/sections/{sectionId}/seats/{seatId}")]
    public Task<VenueDetailsDto> DeleteSeatAsync(Guid venueId, Guid sectionId, Guid seatId)
        => _mediator.Send(new DeleteSeatCommand
            {
                VenueId = venueId,
                SectionId = sectionId,
                SeatId = seatId
            });

    #endregion
}