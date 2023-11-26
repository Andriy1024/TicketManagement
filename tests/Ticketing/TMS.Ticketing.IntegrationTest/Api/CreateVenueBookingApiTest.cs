using System.Net.Http.Json;

using TMS.Common.Users;

using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.VenueBookings;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.Factories;
using TMS.Ticketing.Persistence;

namespace TMS.Ticketing.IntegrationTest.Api;

public class CreateVenueBookingApiTest : IClassFixture<MongoDbFactory>
{
    private readonly TicketingApiFactory _apiFactory;

    public CreateVenueBookingApiTest(MongoDbFactory mongo)
    {
        _apiFactory = new TicketingApiFactory(new MongoConfig
        {
            ConnectionString = mongo.ConnectionString,
            DatabaseName = Guid.NewGuid().ToString()
        });
    }

    [Fact]
    public async Task CreateVenueBooking_ByDefault_CreatesBooking()
    {
        var seedResult = await SeedDataAsync(_apiFactory.Services);

        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(1);
        var createBooking = new CreateVenueBookingCommand
        {
            VenueId = seedResult.VenueId,
            EventId = seedResult.EventId,
            Start = startDate,
            End = endDate
        };

        var client = _apiFactory.CreateApiClient();

        var httpResponse = await client.PostAsJsonAsync("api/venues/book", createBooking);

        var response = await httpResponse.Content.ReadFromJsonAsync<VenueBookingDto>();

        response.Should().NotBeNull();
        response!.EventId.Should().Be(createBooking.EventId);
        response.VenueId.Should().Be(createBooking.VenueId);
        response.Start.Should().Be(createBooking.Start);
        response.End.Should().Be(createBooking.End);
        response.Id.Should().NotBe(Guid.Empty);
    }

    private async Task<(Guid VenueId, Guid EventId)> SeedDataAsync(IServiceProvider serviceProvider) 
    {
        var venueId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        using (var scope = serviceProvider.CreateScope())
        {
            var eventsRepo = scope.ServiceProvider.GetRequiredService<IEventsRepository>();
            
            var venuesRepo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

            var venue = VenueTestFactory.Create(venueId: venueId, name: "Venue #4");
            
            var @event = new EventEntity
            {
                Id = eventId,
                Name = "Event #2",
                CreatorId = UserContext.DefaultId,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(1)
            };

            await venuesRepo.AddAsync(venue);

            await eventsRepo.AddAsync(@event);
        }

        return (venueId, eventId);
    }
}