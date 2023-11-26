using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.Services.Payments;
using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.Factories;
using TMS.Ticketing.IntegrationTest.UseCases.Venues.Common;

namespace TMS.Ticketing.IntegrationTest.UseCases.Venues;

[Collection(VenuesDatabaseCollection.Name)]
public class GetVenuesOverviewTest
{
    private readonly TicketingServices _services;

    public GetVenuesOverviewTest(MongoDbFactory mongoDb)
    {
        _services = new TicketingServices()
          .AddJsonConfig("appsettings", "appsettings.test.json")
          .AddMongoConnection(mongoDb.ConnectionString, Guid.NewGuid().ToString())
          .BuildConfiguration()
          .AddTicketingServices()
          .OverrideService(Substitute.For<IPaymentsService>());
    }

    [Fact]
    public async Task GetVenuesOverview_ByDefaultReturns_VenueOverviewDtos()
    {
        // Arrange
        var services = _services.BuildServiceProvider();

        await SeedDataAsync(services);

        IEnumerable<VenueOverviewDto> actual;

        // Act
        using (var scope = services.CreateScope()) 
        {
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<GetVenuesOverview, IEnumerable<VenueOverviewDto>>>();

            actual = await handler.Handle(new GetVenuesOverview(), CancellationToken.None);
        }

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);

        actual.Should().MatchSnapshot(nameof(actual), matchOptions: o => 
        {
            o.IgnoreField($"**.{nameof(VenueOverviewDto.Id)}");

            return o;
        });
    }

    private static async Task SeedDataAsync(IServiceProvider services) 
    {
        using var scope = services.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

        var firstVenue = VenueTestFactory.Create(name: "Venue #1");
        var secondVenue = VenueTestFactory.Create(name: "Venue #2");

        await repo.AddAsync(firstVenue);
        await repo.AddAsync(secondVenue);
    }
}