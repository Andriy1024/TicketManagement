using System.Net.Http.Json;

using TMS.Common.Users;
using TMS.MongoDB;
using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;

namespace TMS.Ticketing.IntegrationTest.Api;

[Collection(MongoDBReplicaSetCollection.Name)]
public class AddItemToCartApiTest
{
    private readonly TicketingApiFactory _apiFactory;

    public AddItemToCartApiTest(MongoReplicaSetFactory mongo)
    {
        _apiFactory = new TicketingApiFactory(new MongoConfig
        {
            ConnectionString = mongo.ConnectionString,
            DatabaseName = Guid.NewGuid().ToString()
        });
    }

    [Fact]
    public async Task AddItemToCart_ByDefault_AddsItem()
    {
        // Arrange
        var @event = await SeedDataAsync(_apiFactory.Services);

        var offer = @event.Offers.Last();
        var price = @event.Prices.Single(x => x.Id == offer.PriceId);

        var command = new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            EventId = @event.Id,
            SeatId = offer.SeatId,
            PriceId = offer.PriceId
        };

        // Act
        using var client = _apiFactory.CreateApiClient();

        var httpResponse = await client.PostAsJsonAsync("api/orders/carts", command);

        var cartDetails = await httpResponse.Content.ReadFromJsonAsync<CartDetailsDto>();

        // Assert
        cartDetails.Should().NotBeNull();
        cartDetails!.Id.Should().NotBe(Guid.Empty);
        cartDetails.AccountId.Should().Be(UserContext.DefaultId);
        cartDetails.Total.Should().Be(price.Amount);
        cartDetails.OrderItems.Should().HaveCount(1);
        cartDetails.OrderItems.Should().SatisfyRespectively(x => 
        {
            x.EventId.Should().Be(@event.Id);
            x.SeatId.Should().Be(offer.SeatId);
            x.PriceId.Should().Be(offer.PriceId);
            x.Amount.Should().Be(price.Amount);
        });
    }

    private static async Task<EventEntity> SeedDataAsync(IServiceProvider serviceProvider) 
    {
        var venue = FakeVenueFactory.Create(venueId: Guid.NewGuid(), name: "Venue #4");

        var start = DateTime.UtcNow.Date;
        var end = start.AddDays(1);

        var @event = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Event #2",
            CreatorId = UserContext.DefaultId,
            Start = start,
            End = end,
            Details = new List<Detail>() 
            { 
                new() { Name = "Detail 1", Value = "Value 1" } 
            }
        };

        var booking = VenueBookingEntity.Create(
            Array.Empty<VenueBookingEntity>(),
            start, end, venue, @event);

        @event.GeneratePrices().GenerateOffers();

        using (var scope = serviceProvider.CreateScope()) 
        {
            var eventsRepo = scope.ServiceProvider.GetRequiredService<IEventsRepository>();
            var venuesRepo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();
            var bookingRepo = scope.ServiceProvider.GetRequiredService<IVenuesBookingRepository>();

            await venuesRepo.AddAsync(venue);
            await eventsRepo.AddAsync(@event);
            await bookingRepo.AddAsync(booking);
        }

        return @event;
    }
}