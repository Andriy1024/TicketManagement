using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using System.Net;

using TMS.Common.Errors;
using TMS.Test.Common;
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
        var venueId = Guid.NewGuid();

        await SeedDataAsync(venueId);

        VenueDetailsDto actual;

        // Act
        using (var scope = _services.BuildServicesScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<GetVenueDetails, VenueDetailsDto>>();

            actual = await handler.Handle(new GetVenueDetails(venueId), CancellationToken.None);
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
        await SeedDataAsync(Guid.NewGuid());

        Exception? actual = null;

        // Act
        using (var scope = _services.BuildServicesScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<GetVenueDetails, VenueDetailsDto>>();

            try
            {
                _ = await handler.Handle(new GetVenueDetails(Guid.NewGuid()), CancellationToken.None);
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

    private async Task SeedDataAsync(Guid venueId)
    {
        using var scope = _services.BuildServicesScope();

        var repo = scope.ServiceProvider.GetRequiredService<IVenuesRepository>();

        var firstVenue = VenueTestFactory.Create(venueId: venueId, name: "Venue #3");

        await repo.AddAsync(firstVenue);
    }
}