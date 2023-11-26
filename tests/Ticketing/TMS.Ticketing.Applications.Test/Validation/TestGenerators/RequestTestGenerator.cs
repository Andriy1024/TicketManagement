namespace TMS.Ticketing.Applications.Test.Validation.TestGenerators;

public record RequestTestCase(bool SuccessCase, IValidatable Payload);

public class RequestTestGenerator : TestGenerator<RequestTestCase>
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