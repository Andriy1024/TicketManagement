using System.Net.Http.Json;

using TMS.Common.Users;

using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.VenueBookings;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;
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
        // Arrange
        var seedResult = await SeedDataAsync(_apiFactory.Services);

        var startDate = DateTime.UtcNow.Date;
        var createBooking = new CreateVenueBookingCommand
        {
            VenueId = seedResult.VenueId,
            EventId = seedResult.EventId,
            Start = startDate,
            End = startDate.AddDays(1).Date
        };

        // Act
        using var client = _apiFactory.CreateApiClient();
        var httpResponse = await client.PostAsJsonAsync("api/venues/book", createBooking);
        var response = await httpResponse.Content.ReadFromJsonAsync<VenueBookingDto>();

        // Assert
        response.Should().NotBeNull();
        response!.EventId.Should().Be(createBooking.EventId);
        response.VenueId.Should().Be(createBooking.VenueId);
        response.Start.Should().Be(createBooking.Start);
        response.End.Should().Be(createBooking.End);
        response.Id.Should().NotBe(Guid.Empty);
    }

    private static async Task<(Guid VenueId, Guid EventId)> SeedDataAsync(IServiceProvider serviceProvider) 
    {
        var venue = FakeVenueFactory.Create(venueId: Guid.NewGuid(), name: "Venue #4");

        var @event = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Event #2",
            CreatorId = UserContext.DefaultId,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddDays(1)
        };

        using (var scope = serviceProvider.CreateScope())
        {
            var eventsRepo = scope.ServiceProvider.GetRequiredService<IEventsRepository>();
            var venuesRepo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

            await venuesRepo.AddAsync(venue);
            await eventsRepo.AddAsync(@event);
        }

        return (venue.Id, @event.Id);
    }
}