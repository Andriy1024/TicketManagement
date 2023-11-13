using TMS.Ticketing.API.Dtos.Venues;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Persistence.Abstractions;

using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TMS.Ticketing.API.Controllers;

[Route("api/venues")]
[ApiController]
public sealed class VenuesController : ControllerBase
{
    private readonly IMongoRepository<Venue, Guid> _venuesRepo;

    public VenuesController(IMongoRepository<Venue, Guid> venuesRepo)
    {
        _venuesRepo = venuesRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetVenusAsync(CancellationToken token)
    {
        var query = 
            from venue in _venuesRepo.Collection.AsQueryable()
            select new VenueOverviewDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Country = venue.Country,
                City = venue.City,
                Street = venue.Street,
                Details = venue.Details
            };

        return Ok(await query.ToListAsync(token));
    }

    [HttpPost]
    public async Task<IActionResult> CreateVenueAsync([FromBody] VenuePropertiesDto dto)
    {
        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Country = dto.Country,
            City = dto.City,
            Street = dto.Street,
            Details = dto.Details
        };

        await _venuesRepo.AddAsync(venue);

        return Ok();
    }

    [HttpDelete("{venueId}")]
    public async Task<IActionResult> DeleteVenueAsync(Guid venueId)
    {
        await _venuesRepo.DeleteAsync(venueId);

        return Ok();
    }

    [HttpGet("{venueId}/sections")]
    public async Task<IActionResult> GetSectionsAsync(Guid venueId, CancellationToken token)
    {
        var sections = await _venuesRepo.Collection.AsQueryable()
            .Where(x => x.Id == venueId)
            .SelectMany(x => x.Sections)
            .ToListAsync(token);

        return Ok(sections);
    }

    [HttpPut("{venueId}/sections")]
    public async Task<IActionResult> UpdateSectionsAsync([FromRoute] Guid venueId, [FromBody] List<Section> sections)
    {
        //var venue = await _venuesRepo.GetAsync(venueId);

        //if (venue == null)
        //{
        //    throw AppError
        //        .NotFound("Venue was not found")
        //        .ToException();
        //}

        //venue.Sections = sections;

        //await _venuesRepo.UpdateAsync(venue);

        // TODO: split this endpoint into it's own sections/seats CRUD endpoints

        return Ok();
    }
}