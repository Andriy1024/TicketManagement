namespace TMS.Ticketing.Applications.Test.Validation.Data;

public record RequestData(bool SuccessCase, IValidatable Payload);

public class RequestTestGenerator : TestDataCollection<RequestData>
{
    protected void ValidCase(IValidatable payload)
    {
        _testCases.Add(new(true, payload));
    }

    protected void InvalidCase(IValidatable payload)
    {
        _testCases.Add(new(false, payload));
    }
}