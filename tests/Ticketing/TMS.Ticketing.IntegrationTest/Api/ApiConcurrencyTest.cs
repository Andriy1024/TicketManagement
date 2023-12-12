using TMS.Common.Users;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.Persistence;

using System.Net;

namespace TMS.Ticketing.IntegrationTest.Api;

public class ApiConcurrencyTest : IClassFixture<MongoReplicaSetFactory>
{
    private readonly TicketingApiFactory _apiFactory;

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

    public ApiConcurrencyTest(MongoReplicaSetFactory dbFactory)
    {
        _apiFactory = new TicketingApiFactory(new MongoConfig
        {
            ConnectionString = dbFactory.ConnectionString,
            DatabaseName = Guid.NewGuid().ToString()
        });
    }

    [Fact]
    public async Task CreateOrer_ByDefault_CreatesOrder()
    {
        // Arrange
        var cart = await SeedDataAsync(_apiFactory.Services);

        var numberOfRequests = 10;

        var act = async () =>
        {
            using (var client = _apiFactory.CreateApiClient())
            {
                _semaphore.Wait();

                var httpResponse = await client.PostAsync($"api/orders/carts/{cart.Id}/book", content: null);

                var strResponse = await httpResponse.Content.ReadAsStringAsync();

                return (httpResponse.StatusCode, strResponse);
            }
        };

        // Act
        var tasks = Enumerable.Range(0, numberOfRequests).Select(x => Task.Run(() => act()));

        await Task.Delay(TimeSpan.FromSeconds(2));

        _semaphore.Release(numberOfRequests);

        var httpResults = await Task.WhenAll(tasks);

        var successResults = httpResults.Where(x => x.StatusCode == HttpStatusCode.OK).ToArray();

        var orders = await GetOrdersAsync(_apiFactory.Services);

        // Assert
        successResults.Should().HaveCount(1);
        
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
        var venue = FakeVenueFactory.Create(venueId: Guid.Parse("3cdd7140-2333-4d85-a75e-a1ea27125413"), name: "Venue #44");

        var start = DateTime.UtcNow.Date;
        var end = start.AddDays(1);

        var @event = new EventEntity
        {
            Id = Guid.Parse("4bbc7579-9808-41fc-a3c5-004338114bf3"),
            Name = "Event #44",
            CreatorId = UserContext.DefaultId,
            Start = start,
            End = end,
            Details = new List<Detail>()
            {
                new() { Name = "Detail 44", Value = "Value 1" }
            }
        };

        var booking = VenueBookingEntity.Create(
            Array.Empty<VenueBookingEntity>(),
            start, end,
            venue, @event);

        @event.GeneratePrices().GenerateOffers();

        var cart = new CartEntity
        {
            Id = Guid.Parse("d094312b-3bf9-484e-b940-f590d9705622"),
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
        using var scope = serviceProvider.CreateScope();

        return await scope.ServiceProvider
            .GetRequiredService<IOrdersRepository>()
            .GetAllAsync();
    }
}