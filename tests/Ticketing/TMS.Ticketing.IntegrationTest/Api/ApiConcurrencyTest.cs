using TMS.Common.Users;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Venues;

using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.Orders;

using TMS.Ticketing.IntegrationTest.Common.FakeObjects;
using TMS.Ticketing.IntegrationTest.Common;

using TMS.Ticketing.Persistence;

using System.Net.Http.Json;
using System.Net;
using System.Threading;

namespace TMS.Ticketing.IntegrationTest.Api;

public class ApiConcurrencyTest
{
    private readonly TicketingApiFactory _apiFactory;

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

    public ApiConcurrencyTest()
    {
        _apiFactory = new TicketingApiFactory(new MongoConfig
        {
            ConnectionString = "mongodb://127.0.0.1:27017/?replicaSet=dbrs",
            DatabaseName = "TMS_Concurrency_Test"
        });
    }

    [Fact]
    public async Task CreateOrer_ByDefault_CreatesOrder()
    {
        // Arrange
        var cart = await SeedDataAsync(_apiFactory.Services);

        // Act
        var tasks = Enumerable
            .Range(0, 10)
            .Select(x =>
                Task.Run(() => RunRequest(_apiFactory, cart))
            );

        Console.WriteLine("Before Delay");

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("After Delay");

        _semaphore.Release(10);

        var httpResults = await Task.WhenAll(tasks);

        var successResults = httpResults.Where(x => x.Code == HttpStatusCode.OK).ToArray();

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

    private record HttpTestResult(HttpStatusCode Code, string Response);

    private async Task<HttpTestResult> RunRequest(TicketingApiFactory apiFactory, CartEntity cart)
    {
        using var client = _apiFactory.CreateApiClient();

        _semaphore.Wait();

        var httpResponse = await client.PostAsync($"api/orders/carts/{cart.Id}/book", content: null);

        var strResponse = await httpResponse.Content.ReadAsStringAsync();

        return new HttpTestResult(httpResponse.StatusCode, strResponse);
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

public class SafeResult
{
    public Exception? Error { get; private set; }

    public bool IsFailed => Error != null;

    public static SafeResult Failed(Exception? error) => new() { Error = error };

    public static SafeResult Success() => new();
}

//public static class SafeExtension
//{
//    public static SafeResult Safe(Func<Task> act)
//    {
//        try
//        {
//            await act();

//            return SafeResult.Success();
//        }
//        catch (Exception e)
//        {
//            return SafeResult.Failed(e);
//        }
//    }
//}