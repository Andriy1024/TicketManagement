using System.Net;

using TMS.Ticketing.Applications.Test.Validation.Data;

namespace TMS.Ticketing.Applications.Test.Validation;

public class RequestValidationTest
{
    [Theory]
    [ClassData(typeof(VenueRequestData))]
    [ClassData(typeof(VenueBookingRequestData))]
    [ClassData(typeof(PriceRequestData))]
    [ClassData(typeof(OrderRequestData))]
    [ClassData(typeof(OfferRequestData))]
    [ClassData(typeof(EventRequestData))]
    [ClassData(typeof(CartRequestData))]
    public void Request_IsValidated_As_Expected(RequestData testCase)
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