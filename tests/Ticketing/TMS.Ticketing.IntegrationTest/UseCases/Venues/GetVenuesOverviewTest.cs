using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;

namespace TMS.Ticketing.IntegrationTest.UseCases.Venues;

[Collection(MongoDBCollection.Name)]
public class GetVenuesOverviewTest
{
    private readonly TicketingServicesBuilder _services;

    public GetVenuesOverviewTest(MongoDBFactory mongoDb)
    {
        _services = new TicketingServicesBuilder()
          .AddJsonConfig("appsettings", "appsettings.test.json")
          .AddMongoConnection(mongoDb.ConnectionString, Guid.NewGuid().ToString())
          .BuildConfiguration()
          .AddTicketingServices()
          .SetFakeCache()
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
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            actual = await mediator.Send(new GetVenuesOverview(), CancellationToken.None);
        }

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);

        actual.Should().MatchSnapshot(nameof(actual), matchOptions: o => 
        {
            return o.IgnoreField($"**.{nameof(VenueOverviewDto.Id)}");
        });
    }

    private static async Task SeedDataAsync(IServiceProvider services) 
    {
        using var scope = services.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

        var firstVenue = FakeVenueFactory.Create(name: "Venue #1");
        var secondVenue = FakeVenueFactory.Create(name: "Venue #2");

        await repo.AddAsync(firstVenue);
        await repo.AddAsync(secondVenue);
    }
}