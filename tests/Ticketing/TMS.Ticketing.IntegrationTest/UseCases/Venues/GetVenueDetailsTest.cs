using System.Net;

using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.Services.Payments;
using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.Factories;
using TMS.Ticketing.IntegrationTest.UseCases.Venues.Common;

namespace TMS.Ticketing.IntegrationTest.UseCases.Venues;

[Collection(VenuesDatabaseCollection.Name)]
public class GetVenueDetailsTest
{
    private readonly TicketingServices _services;

    private readonly static Guid DatabaseName = Guid.NewGuid();

    public GetVenueDetailsTest(MongoDbFactory mongoDb)
    {
        _services = new TicketingServices()
          .AddJsonConfig("appsettings", "appsettings.test.json")
          .AddMongoConnection(mongoDb.ConnectionString, DatabaseName.ToString())
          .BuildConfiguration()
          .AddTicketingServices()
          .OverrideService(Substitute.For<IPaymentsService>());
    }

    [Fact]
    public async Task GetVenueDetails_ByDefaultReturns_VenueDetailsDto()
    {
        // Arrange
        var services = _services.BuildServiceProvider();

        var venueId = Guid.NewGuid();

        await SeedDataAsync(services, venueId);

        VenueDetailsDto actual;

        // Act
        using (var scope = services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            actual = await mediator.Send(new GetVenueDetails(venueId), CancellationToken.None);
        }

        // Assert
        actual.Should().NotBeNull();

        actual.Should().MatchSnapshot(nameof(actual), matchOptions: o =>
        {
            o.IgnoreField($"**.{nameof(VenueOverviewDto.Id)}");
            o.IgnoreField($"**.{nameof(SectionDto.VenueId)}");
            o.IgnoreField($"**.{nameof(SectionDto.SectionId)}");
            o.IgnoreField($"**.{nameof(SeatDto.SeatId)}");

            return o;
        });
    }

    [Fact]
    public async Task GetVenueDetails_NotExistedVenue_ThrowsNotFoundError()
    {
        // Arrange
        var services = _services.BuildServiceProvider();

        await SeedDataAsync(services, Guid.NewGuid());

        Exception? actual = null;

        // Act
        using (var scope = services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                _ = await mediator.Send(new GetVenueDetails(Guid.NewGuid()), CancellationToken.None);
            }
            catch (Exception e)
            {
                actual = e;
            }
        }

        // Assert
        actual.Should().NotBeNull()
            .And.BeAssignableTo<ApiException>()
            .Subject.Error
            .Should().Match<ApiError>(x => x.StatusCode == HttpStatusCode.NotFound);
    }

    private static async Task SeedDataAsync(IServiceProvider services, Guid venueId)
    {
        using var scope = services.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

        var firstVenue = VenueTestFactory.Create(venueId: venueId, name: "Venue #3");

        await repo.AddAsync(firstVenue);
    }
}