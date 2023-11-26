using System.Collections;

namespace TMS.Test.Common;

public class TestGenerator<TTest> : IEnumerable<object[]>
    where TTest : notnull
{
    protected readonly List<TTest> _testCases = new();

    public IEnumerator<object[]> GetEnumerator()
    {
        return _testCases.Select(x => new object[] { x }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}