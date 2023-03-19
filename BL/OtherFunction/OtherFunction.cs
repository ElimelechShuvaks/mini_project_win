
namespace OtherFunction;

internal static class OtherFunction
{
    internal static string GetToStrings<T>(this IEnumerable<T> values)
    {
        return string.Join('\n', values);
    }
}
