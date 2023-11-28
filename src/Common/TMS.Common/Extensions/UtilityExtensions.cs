using System.Runtime.CompilerServices;

namespace TMS.Common.Extensions;

public static class UtilityExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        => source == null || !source.Any();

    public static string ThrowIfNullOrEmpty(this string? text, [CallerArgumentExpression("text")] string? paramName = null)
        => string.IsNullOrEmpty(text)
            ? throw new ArgumentNullException(paramName)
            : text;
}