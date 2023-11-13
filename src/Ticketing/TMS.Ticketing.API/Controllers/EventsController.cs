using Microsoft.AspNetCore.Mvc;

using TMS.Common.Errors;
using TMS.Common.Users;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.API.Dtos.Events;
using TMS.Ticketing.Persistence.Abstractions;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TMS.Ticketing.API.Controllers;

[Route("api/events")]
[ApiController]
public sealed class EventsController : ControllerBase
{
    private readonly IMongoRepository<Event, Guid> _eventsRepo;

    public EventsController(IMongoRepository<Event, Guid> eventsRepo)
    {
        _eventsRepo = eventsRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetEventsAsync(CancellationToken token)
    {
        var query =
            from @event in _eventsRepo.Collection.AsQueryable()
            select new EventOverviewDto
            {
                Id = @event.Id,
                Name = @event.Name,
                Details = @event.Details,
                Start = @event.Start,
                End = @event.End
            };

        return Ok(await query.ToListAsync(token));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEventAsync(
        [FromBody] EventPropertiesDto dto, 
        [FromServices] IUserContext user)
    {
        var venue = new Event
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Details = dto.Details,
            Start = dto.Start,
            End = dto.End,
            CreatorId = user.GetUser().Id
        };

        await _eventsRepo.AddAsync(venue);

        return Ok();
    }

    [HttpPut("{eventId}")]
    public async Task<IActionResult> UpdateEventAsync(
        [FromRoute] Guid eventId, 
        [FromBody] EventPropertiesDto dto)
    {
        var @event = await _eventsRepo.GetAsync(eventId);

        if (@event == null)
        {
            throw AppError
                .NotFound("Event was not found")
                .ToException();
        }

        @event.Name = dto.Name;
        @event.Details = dto.Details;
        @event.Start = dto.Start;
        @event.End = dto.End;

        await _eventsRepo.UpdateAsync(@event);

        return Ok();
    }

    [HttpDelete("{eventId}")]
    public async Task<IActionResult> DeleteEventAsync(Guid eventId)
    {
        await _eventsRepo.DeleteAsync(eventId);

        return Ok();
    }
}