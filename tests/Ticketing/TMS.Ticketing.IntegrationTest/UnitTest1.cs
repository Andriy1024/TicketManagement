using FluentAssertions;

namespace TMS.Ticketing.IntegrationTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var str = "s";

        str.Should().Be("s");
    }
}