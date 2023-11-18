namespace TMS.Common.Extensions;

public static class UtilityExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        => source == null || !source.Any();
}
