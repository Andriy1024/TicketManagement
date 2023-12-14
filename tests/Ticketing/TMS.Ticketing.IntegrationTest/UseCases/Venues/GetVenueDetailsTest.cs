using System.Net;

using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Application.Repositories;
using TMS.Ticketing.Application.UseCases.Venues;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.IntegrationTest.Common;
using TMS.Ticketing.IntegrationTest.Common.FakeObjects;

namespace TMS.Ticketing.IntegrationTest.UseCases.Venues;

[Collection(MongoDBCollection.Name)]
public class GetVenueDetailsTest
{
    private readonly TicketingServicesBuilder _services;

    private readonly static Guid DatabaseName = Guid.NewGuid();

    public GetVenueDetailsTest(MongoDBFactory mongoDb)
    {
        _services = new TicketingServicesBuilder()
          .AddJsonConfig("appsettings", "appsettings.test.json")
          .AddMongoConnection(mongoDb.ConnectionString, DatabaseName.ToString())
          .BuildConfiguration()
          .AddTicketingServices()
          .SetFakeCache()
          .OverrideService(Substitute.For<IPaymentsService>());
    }

    [Fact]
    public async Task GetVenueDetails_ByDefaultReturns_VenueDetailsDto()
    {
        // Arrange
        var services = _services.BuildServiceProvider();

        var venue = await SeedDataAsync(services);

        VenueDetailsDto actual;

        // Act
        using (var scope = services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            actual = await mediator.Send(new GetVenueDetails(venue.Id), CancellationToken.None);
        }

        // Assert
        actual.Should().NotBeNull();

        actual.Should().MatchSnapshot(nameof(actual), matchOptions: opt => opt
            .IgnoreField($"**.{nameof(VenueOverviewDto.Id)}")
            .IgnoreField($"**.{nameof(SectionDto.VenueId)}")
            .IgnoreField($"**.{nameof(SectionDto.SectionId)}")
            .IgnoreField($"**.{nameof(SeatDto.SeatId)}"));
    }

    [Fact]
    public async Task GetVenueDetails_NotExistedVenue_ThrowsNotFoundError()
    {
        // Arrange
        var services = _services.BuildServiceProvider();

        _ = await SeedDataAsync(services);

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

    private static async Task<VenueEntity> SeedDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

        var venue = FakeVenueFactory.Create(venueId: Guid.NewGuid(), name: "Venue #3");

        await repo.AddAsync(venue);

        return venue;
    }
}