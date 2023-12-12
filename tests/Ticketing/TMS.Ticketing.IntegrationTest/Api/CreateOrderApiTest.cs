using System.Net.Http.Json;

using TMS.Common.Users;

using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.Orders;
using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;
using TMS.Ticketing.Persistence;

namespace TMS.Ticketing.IntegrationTest.Api;

[Collection(MongoDBReplicaSetCollection.Name)]
public class CreateOrderApiTest
{
    private readonly TicketingApiFactory _apiFactory;

    public CreateOrderApiTest(MongoReplicaSetFactory mongo)
    {
        _apiFactory = new TicketingApiFactory(new MongoConfig
        {
            ConnectionString = mongo.ConnectionString,
            DatabaseName = Guid.NewGuid().ToString()
        });
    }

    [Fact]
    public async Task CreateOrer_ByDefault_CreatesOrder()
    {
        // Arrange
        var cart = await SeedDataAsync(_apiFactory.Services);

        // Act
        using var client = _apiFactory.CreateApiClient();

        var httpResponse = await client.PostAsync($"api/orders/carts/{cart.Id}/book", content: null);

        var createOrderResult = await httpResponse.Content.ReadFromJsonAsync<CreateOrderCommandResult>();

        var orders = await GetOrdersAsync(_apiFactory.Services);

        // Assert
        createOrderResult.Should().NotBeNull();
        createOrderResult!.PaymentId.Should().NotBe(Guid.Empty);
        orders.Should().HaveCount(1);
        orders.Should().SatisfyRespectively(x =>
        {
            x.Total.Should().Be(cart.Total);
            x.AccountId.Should().Be(cart.AccountId);
            x.OrderItems.Should().BeEquivalentTo(cart.OrderItems);
        });
    }

    private static async Task<CartEntity> SeedDataAsync(IServiceProvider serviceProvider)
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
            start, end,
            venue, @event);

        @event.GeneratePrices().GenerateOffers();

        var cart = new CartEntity
        {
            Id = Guid.NewGuid(),
            AccountId = UserContext.DefaultId,
        };

        var cartItems = 
            from offer in @event.Offers.DistinctBy(x => x.SeatId)
            join price in @event.Prices 
                on offer.PriceId equals price.Id
            select new OrderItem
            {
                EventId = @event.Id,
                SeatId = offer.SeatId,
                PriceId = offer.PriceId,
                Amount = price.Amount
            };

        cart.OrderItems = cartItems.ToList();

        using (var scope = serviceProvider.CreateScope())
        {
            var eventsRepo = scope.ServiceProvider.GetRequiredService<IEventsRepository>();
            var venuesRepo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();
            var bookingRepo = scope.ServiceProvider.GetRequiredService<IVenuesBookingRepository>();
            var cartRepo = scope.ServiceProvider.GetRequiredService<ICartsRepository>();

            await venuesRepo.AddAsync(venue);
            await eventsRepo.AddAsync(@event);
            await bookingRepo.AddAsync(booking);
            await cartRepo.AddAsync(cart);
        }

        return cart;
    }

    private static async Task<IEnumerable<OrderEntity>> GetOrdersAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var ordersRepo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

            return await ordersRepo.GetAllAsync();
        }
    }
}
