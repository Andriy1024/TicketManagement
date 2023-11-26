using System.Net;

using TMS.Ticketing.Applications.Test.Validation.TestGenerators;

namespace TMS.Ticketing.Applications.Test.Validation;

public class RequestValidationTest
{
    [Theory]
    [ClassData(typeof(VenueRequestTestGenerator))]
    [ClassData(typeof(VenueBookingRequestTestGenerator))]
    [ClassData(typeof(PriceRequestTestGenerator))]
    [ClassData(typeof(OrderRequestTestGenerator))]
    [ClassData(typeof(OfferRequestTestGenerator))]
    [ClassData(typeof(EventRequestTestGenerator))]
    [ClassData(typeof(CartRequestTestGenerator))]
    public void Request_Validates_AsExpected(RequestTestCase testCase)
    {
        // Act
        var act = () => testCase.Payload.Validate().ThrowIfInvalid();

        // Assert
        if (testCase.SuccessCase)
        {
            act.Should().NotThrow();

            return;
        }

        var ex = act.Should()
            .Throw<ApiException>()
            .And.Error.Should().NotBeNull()
            .And.Match<ApiError>(
                x => x.StatusCode == HttpStatusCode.BadRequest);
    }
}